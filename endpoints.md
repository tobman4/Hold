# API Endpoints

This file documents the API endpoints available in the application.

## Hold Items

Base path: `/hold`

### `GET /hold`
Retrieves a list of all hold items.

### `GET /hold/{id:guid}`
Retrieves a specific hold item by its unique identifier (GUID). Returns a 404 Not Found if the item does not exist.

### `POST /hold`
Creates a new hold item. Returns a 201 Created with the new item and its location if successful.

### `PUT /hold/{id:guid}`
Updates an existing hold item by its unique identifier (GUID). Returns a 404 Not Found if the item does not exist.

### `DELETE /hold/{id:guid}`
Deletes a specific hold item by its unique identifier (GUID). Returns a 404 Not Found if the item does not exist.
