# ASCOM.Alpaca.Simulators

An Alpaca Device Server built using ASP.Net 5, MVC and Blazor. The program creates and manages the controllers for the Alpaca devices as well as UIs for configuration.

Currently the project exposes one of each ASCOM Alpaca Device type. Device settings are provided by the Alpaca Settings endpoint for the device and are accessible from the menu. 

The current build uses the default development port of 32323. As development continues this may be changed.

Discovery uses the ASCOM Standard library and the standard discovery port of 32227. By default discovery is turned on.

The project auto-generates Swagger pages while running. The Swagger pages shows all active controllers in the project by default. Because this exposes all controller types the Swagger page has all Alpaca Endpoints.

Authentication is optional and can be turned on from the setup page. This currently supports HTTP basic authentication and token based authentication.