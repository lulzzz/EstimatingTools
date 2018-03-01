﻿using System.Collections.Generic;

namespace EstimatingLibrary
{
    public static class Defaults
    {
        public static List<string> Scope = new List<string>(new string[]
        {
            "Provide a seamless extension to the existing Honeywell system provided by T.E.C.Systems.",
            "System components shall be manufactured by Honeywell Automation and will be designed and installed by T.E.C. Systems, Inc.",
            "Provide the engineering resources required for the creation of design, installation and as-built documentation as required for the submittal and training phases of the project.",
            "Provide the programming resources required.",
            "Provide field technical resources required for system commissioning, calibration, start-up and testing.",
            "Provide training in accordance with specification requirements.",
            "Provide one-year parts and labor warranty."
        });

        public static List<string> Notes = new List<string>(new string[]
        {
            "Use tax is included; this project is assumed to be a capital improvement project."
        });

        public static List<string> Exclusions = new List<string>(new string[]
        {
            "Overtime labor.",
            "Power wiring.",
            "Access doors.",
            "Demolition.",
            "Patching, painting and debris removal.",
            "Automatic louver dampers, smoke dampers, fire/smoke dampers, and end switches.",
            "Steam-fitting and Sheet-metal labor.",
            "Installation of valves, wells, and dampers.",
            "Self-contained valves.",
            "Provision of smoke detection and fire alarm system components.",
            "Payment and Performance Bonds.",
            "Smoke purge and UL-864 systems."
        });
    }
}
