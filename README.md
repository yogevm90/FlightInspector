# FlightInspector

Special notes:
- Flight Inspector consists of 4 views (along with their VMs and models) - The main view, the flight data view and the playback view.
- All models are connected to the same SimulationRunner (which is responsible to open, config and send data to the FlighGear simulator).
- Each view-vm-model is independent of the others, and share only the data being stored in the SimulationRunner.

Project structure:
- All data is located in the same solution project, whithout extensions needed (except the framework itself)
- Flight csv data files and config xml files can be stored where user wants, you just need to choose the files location on the configuration windows before starting the simulation.
- example files located in the project.

Framework:
- Project consists of only .NET Framework 4.7.2, No need to install any other components (except FlightGear of course)

Installation:
- Inside bin folder you will find a compiled version of the code, you can either run the exe there or recompile solution.
