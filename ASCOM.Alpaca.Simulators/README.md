ASCOM.Alpaca.Simulators

An Alpaca Device Server built using ASP.Net 5. This creates and manages the controllers for the Alpaca devices as well as UIs for configuration.

Currently the project exposes one of each ASCOM Device type. Device settings are provided by the Alpaca Settings endpoint for the device and are accessible from the menu. 

The current beta uses the default development port of 5000. As development continues this will be changed to the final correct port.

Discovery uses the ASCOM Standard library and the standard discovery port of 32227. By default discovery is turned on.

The project auto-generates Swagger pages while running. The Swagger pages shows all active controllers in the project by default. Because this exposes all controller types the Swagger page has all Alpaca Endpoints.