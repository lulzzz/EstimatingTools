﻿using EstimatingLibrary;
using EstimatingLibrary.Interfaces;
using EstimatingLibrary.Utilities;
using NLog;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace EstimatingLibrary.Utilities
{
    public class DoStacker
    {
        static private Logger logger = LogManager.GetCurrentClassLogger();

        private List<TECChangedEventArgs> undoStack;
        private List<TECChangedEventArgs> redoStack;
        private ChangeWatcher watcher;
        private bool isDoing = false;

        #region Constructors
        public DoStacker(ChangeWatcher changeWatcher)
        {
            watcher = changeWatcher;
            watcher.Changed += objectChanged;

            undoStack = new List<TECChangedEventArgs>();
            redoStack = new List<TECChangedEventArgs>();
        }
        
        #endregion

        private void objectChanged(TECChangedEventArgs e)
        {
            if (!isDoing) { redoStack.Clear(); }
            if(e.Sender is IDoRedoable doable && doable.CanDo)
            {
                undoStack.Add(e);
            }
        }

        public void Undo()
        {
            isDoing = true;
            TECChangedEventArgs item = undoStack.Last();
            logger.Trace("Undoing:       " + item.Change.ToString() + "    #Undo: " + undoStack.Count() + "    #Redo: " + redoStack.Count());
            if (item.Change == Change.Add)
            {
                handleAdd(item);
                undoStack.Remove(item);
                undoStack.Remove(undoStack.Last());
                redoStack.Add(item);
            }
            else if (item.Change == Change.Remove)
            {
                handleRemove(item);
                undoStack.Remove(item);
                undoStack.Remove(undoStack.Last());
                redoStack.Add(item);
            }
            else if (item.Change == Change.Edit)
            {
                int index = undoStack.IndexOf(item);
                redoStack.Add(new TECChangedEventArgs(Change.Edit, item.PropertyName, item.Sender, item.OldValue, item.Value));
                handleEdit(item);
                for (int x = (undoStack.Count - 1); x >= index; x--)
                {
                    undoStack.RemoveAt(x);
                }
            }

            string message = "After Undoing: " + item.Change.ToString() + "    #Undo: " + undoStack.Count() + "    #Redo: " + redoStack.Count() + "\n";
            logger.Trace(message);

            isDoing = false;
        }
        public void Redo()
        {
            isDoing = true;
            TECChangedEventArgs item = redoStack.Last();

            string message = "Redoing:       " + item.Change.ToString() + "    #Undo: " + undoStack.Count() + "    #Redo: " + redoStack.Count();
            logger.Trace(message);

            if (item.Change == Change.Add)
            {
                handleRemove(item);
                redoStack.Remove(item);
            }
            else if (item.Change == Change.Remove)
            {
                handleAdd(item);
                redoStack.Remove(item);
            }
            else if (item.Change == Change.Edit)
            {
                int index = 0;
                if (undoStack.Count > 0)
                {
                    index = undoStack.IndexOf(undoStack.Last());
                }

                handleEdit(item);
                redoStack.Remove(item);
                for (int x = (undoStack.Count - 2); x > index; x--)
                {
                    undoStack.RemoveAt(x);
                }
            }

            message = "After Redoing: " + item.Change.ToString() + "    #Undo: " + undoStack.Count() + "    #Redo: " + redoStack.Count() + "\n";
            logger.Trace(message);

            isDoing = false;
        }

        private void handleAdd(TECChangedEventArgs item)
        {
            try
            {
                (item.Sender as IDoRedoable).RemoveForProperty(item.PropertyName, item.Value);
            }
            catch
            {
                string message = "Target object: " + item.Sender + " and reference object " + item.Value + " not handled in add";
                logger.Trace(message);
            }
        }
        private void handleRemove(TECChangedEventArgs item)
        {
            try
            {
                (item.Sender as IDoRedoable).AddForProperty(item.PropertyName, item.Value);

            }
            catch
            {
                string message = "Target object: " + item.Sender + " and reference object " + item.Value + " not handled in remove";
                logger.Trace(message);
            }
        }
        private void handleEdit(TECChangedEventArgs item)
        {
            try
            {
                (item.Sender as IDoRedoable).SetProperty(item.PropertyName, item.OldValue);
            }
            catch
            {
                string message = "Property could not be set: " + item.PropertyName;
                logger.Error(message);
            }
            
        }

        public int UndoCount()
        {
            return undoStack.Count;
        }
        public int RedoCount()
        {
            return redoStack.Count;
        }

    }
}
