# Products Manager APP
Web app used for products management.

## Commands ##
All commands are in Makefile
```shell
1) make help
```
## Install Dependencies  ##
```shell
1) make install 
```

## Run Local  ##
```shell
1) make run 
```

## Run Build ##
```shell
1) make build
```

### Software ##
- React JS
- Typescript

### Components ##
- Shadcn ui 
Documentation: https://ui.shadcn.com/

# SGP_F

SGP_F is a web application built with React, Vite, and TypeScript. This project uses Tailwind CSS for styling and includes several reusable components and pages for managing categories, products, and stock.

## Getting Started

Follow these steps to set up and run the application locally:

### 1. Install Dependencies

Make sure you have [Node.js](https://nodejs.org/) installed. Then, install the project dependencies:

```bash
npm install
```

### 2. Configure Environment Variables

Create a `.env` file in the root directory to store environment-specific variables. Example:

```env
VITE_API_URL=http://localhost:5891/api
```

- `VITE_API_URL`: The base URL for the backend API.
- You can add other variables as needed for your environment.

### 3. Run the Application

Start the development server:

```bash
npm run dev
```

The app will be available at [http://localhost:5173](http://localhost:5173) by default.

### 4. Build for Production

To build the application for production:

```bash
npm run build
```

The output will be in the `dist/` folder.

### 5. Linting

To check for linting errors:

```bash
npm run lint
```

## Project Structure

- `src/` - Source code
  - `components/` - Reusable UI components
  - `pages/` - Application pages (Dashboard, Categories, Products, Stock)
  - `services/` - API service modules
  - `providers/` - Context providers (Auth, Theme)
  - `routes/` - Application routes
  - `hooks/` - Custom React hooks
  - `lib/` - Utility functions
- `public/` - Static assets
- `Dockerfile` - Docker configuration (if needed)
- `Makefile` - Build and run commands

## Additional Notes

- The project uses Vite for fast development and build.
- Tailwind CSS is used for styling.
- Environment variables prefixed with `VITE_` are exposed to the client.
