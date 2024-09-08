# Volume Mixer Backend

This backend API is built in .NET and uses the NAudio package to control the audio levels of programs on a Windows PC. It works in conjunction with the Angular frontend to allow users to adjust their program volumes from any device connected to the same local network.

## Features

- **Program Audio Control**: Adjust the volume of any program running on your Windows PC using the NAudio library.
- **Local Network Access**: The API runs on your PC and can be accessed from any device within the same network.

## Getting Started

1. Clone this repository to your Windows machine.
2. Make sure you have .NET installed (check the required version in the project file).
3. Open the solution in Visual Studio or your preferred IDE.
4. Restore the NuGet packages, including NAudio, by running `dotnet restore`.
5. Build the solution to ensure everything is set up correctly.
6. Run the application using `dotnet run` or start it through your IDE.
7. The API will start on your local machine. Make sure to note the IP address, which will be needed to connect the frontend.
NOTE: May have to accept the dev certificates to allow the frontend to connect.

## Usage

- Once the backend is running, use the Angular frontend to control the audio levels of programs on your PC.
- If you're trying it out for the first time, consider using the demo mode in the frontend to get a feel for the app before connecting it to this backend.

## Tech Stack

- **Frontend**: [Link to frontend](https://github.com/henleyaustin/mixer-web)
- **Backend**: ASP.NET Core
- **Audio Control**: NAudio package

## Notes

This project was built for fun and experience in controlling program volumes on a Windows PC. If you encounter any issues or have suggestions, feel free to open an issue or contribute!

---
