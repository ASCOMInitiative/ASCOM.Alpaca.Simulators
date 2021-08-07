# ASCOM Alpaca Simulators
## About
A test project for Alpaca Simulators. The goal is to be as compliant as possible with the Alpaca specification. By default it starts on localhost:32323. The port may change as it nears release.

This exposes all current Alpaca devices in simulator form. They are direct ports from the ASCOM Platform simulators over to .Net Standard 2.0. Once complete they are meant to be fully compatible with the platform versions. The configuration (which is in progress) is achieved through a Blazor web UI.

Most devices will have a Setup page for device settings and a Control page. The control page allows the device to run without a client, similar to the ASCOM Simulator handboxes.

Settings, discovery, and logging are provided by the ASCOM Standard libraries. The log and settings files can be found in the standard folders.

This supports Swagger / OpenAPI on the /swagger url. Please note that the documentation on the ASCOM website should be considered canonical, to better handle serialization of some endpoints (camera images) the automatic serializer is not used so the swagger doc here shows a string as the return.

There are builds available in the releases section. While in Alpha and Beta major breaking changes may occur on any update. See the release notes for changes and the available platforms. Normally builds should be available for Windows, Linux (desktop and Raspberry Pi) and macOS. 
## Features
## Builds
## Roadmap
## License
## Feedback