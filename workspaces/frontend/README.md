# Patient Manager Frontend

This is the frontend application for the Patient Manager system built with Angular.

## Technologies & Libraries

- **Angular 17**: Frontend framework
- **TailwindCSS**: Utility-first CSS framework for styling
- **@angular/material**: Material Design components for Angular
- **SignalR**: Real-time communication with the backend
- **TypeScript**: Programming language
- **SCSS**: CSS preprocessor
- **PostCSS**: Tool for transforming CSS

## Prerequisites

- Node.js (v18 or higher)
- npm (comes with Node.js)

## Setup & Running

1. Install dependencies:
```bash
npm install
```

2. Start the development server:
```bash
npm start
```
or
```bash
ng serve
```

The application will be available at `http://localhost:4200`

## Building for Production

To build the application for production:

```bash
npm run build
```

The build artifacts will be stored in the `dist/` directory.

## Project Structure

- `src/app/components/`: Contains all Angular components
- `src/app/services/`: Contains services for API communication
- `src/app/dialogs/`: Contains dialog components
- `src/app/guard/`: Contains route guards for authentication
- `src/app/interceptors/`: Contains HTTP interceptors
- `src/app/utils/`: Contains utility functions
- `src/assets/`: Contains static assets like images and icons
- `src/environments/`: Contains environment configuration files
