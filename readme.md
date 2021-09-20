# ASCOM Alpaca Simulators
## About
This project is developing a set of Alpaca Simulators. The goal is to be as compliant as possible with the Alpaca specification. By default it starts on localhost:32323. The port may change as it nears release.

This exposes all current Alpaca devices in simulator form. They are direct ports from the ASCOM Platform simulators over to .Net Standard 2.0. Once complete they are meant to be fully compatible with the platform versions. The configuration (which is in progress) is achieved through a Blazor web UI.

Most devices will have a Setup page for device settings and a Control page. The control page allows the device to run without a client, similar to the ASCOM Simulator handboxes.

Settings, discovery, and logging are provided by the ASCOM Standard libraries. The log and settings files can be found in the standard folders for the ASCOM Standard project.

This supports Swagger / OpenAPI on the /swagger url. Please note that the documentation on the ASCOM website should be considered canonical, to better handle serialization of some endpoints (camera images) the automatic serializer is not used so the swagger doc here shows a string as the return. As development progresses it is expected that the auto-generated Swagger document will better match the Alpaca Specification.

There are builds available in the releases section. While in Alpha and Beta major breaking changes may occur on any update. See the release notes for changes and the available platforms. Normally builds should be available for Windows, Linux (desktop and Raspberry Pi) and macOS.
## Builds

Prebuilt versions are available from the Github releases page (https://github.com/DanielVanNoord/ASCOM.Alpaca.Simulators/releases). These include packages for Windows, Linux (x64, armhf and aarch64) and macOS (x64). Versions with preview in the name are test builds for future releases.
## Roadmap
Feature updates, not in order

1. Improve swagger doc generation and match official specification
2. Add UI function for Telescope and Camera
3. Add a traffic monitor to the UI
4. Add camera to use user supplied images
5. Add temperature simulator to camera
6. Improve synchronous method calls 
7. Add TLS support 
## Feedback

Feedback can be given to the ASCOM Developer forum or here on the Github page. For issues please include any relevant logs. Please note that this is not the appropriate place to request changes to the Alpaca protocols. This is an implementation of the protocols, not the protocols themselves.

## License

