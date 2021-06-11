## ASCOM Alpaca Simulators
A test project for Alpaca Simulators. By default it starts on localhost:5000.

This exposes all current Alpaca devices in simulator form. They are direct ports from the ASCOM Platform simulators over to .Net Standard 2.0. The configuration (which is in progress) is achieved through a Blazor web UI.

Most devices will have a Setup page for device settings and a Control page. The control page allows the device to run without a client, similar to the ASCOM Simulator handboxes.

Settings and logging are provided by the ASCOM Standard libraries.

This supports Swagger / OpenAPI on the /swagger url. Please note that the documentation on the ASCOM website should be considered canonical, to better handle serialization of some endpoints (camera images) the automatic serializer is not used so the swagger doc here shows a string as the return.

There are builds available in the releases section. While in Alpha and Beta major breaking changes may occur on any update. See the release notes for changes and the available platforms. Normally builds should be available for Windows, Linux (desktop and Raspberry Pi) and macOS. 