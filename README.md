# FlightInspector

Special notes:
- Flight Inspector consists of 4 views (along with their VMs and models) - The main view, the flight data view and the playback view.
- All models are connected to the same SimulationRunner (which is responsible to open, config and send data to the FlighGear simulator).
- Each view-vm-model is independent of the others, and share only the data being stored in the SimulationRunner.
- In order to run simulation you will have to first configure in the main windows the simulation installtion dir location, the configuration file location and the flight data file location. After you will be able to run simulator, and after server is up (will be shown in the message box) you can start running sending data to the simulation (by pressing play)
- Any error or issue on the configuration or running the simulator will be shown in the message box on the main window.

Project structure:
- All data is located in the same solution project, whithout extensions needed (except the framework itself)
- Flight csv data files and config xml files can be stored where user wants, you just need to choose the files location on the configuration windows before starting the simulation.
- example files located in the project.

Framework:
- Project consists of only .NET Framework 4.7.2, No need to install any other components (except FlightGear of course)

Installation:
- Inside 'Executable' folder you will find a compiled version of the code, you can either run the exe there or recompile solution.

Diagrams:
- Project diagrams located on main folder - in png and in cd format (to look inside solution)
