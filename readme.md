## ASCOM Alpaca Simulators
A test project for Alpaca Simulators.

This exposes all current Alpaca devices in simulator form. They are direct ports from the ASCOM Platform simulators over to .Net Standard 2.0. The configuration (which is in progress) is achieved through a Blazor web UI.

This supports Swagger / OpenAPI on the /swagger url. Please note that the documentation on the ASCOM website should be considered canonical, to better handle serialization of some endpoints (camera images) the  automatic serializer is not used so the swagger doc here shows a string as the return.