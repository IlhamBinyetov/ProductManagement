# ProductManagement  Api Project
using SQLite as a database


Endpoints

Products
GET /api/Product/GetAllProducts: Get all products.
GET /api/Product/GetProduct{id}: Get a product by ID.
POST /api/Product/AddProduct: Create a new product.
PUT /api/Product/UpdateProduct/{id}: Update an existing product.
DELETE /api/Product/DeleteProduct/{id}: Delete a product.

Customers
GET /api/Customer/GetAllCustomers: Get all Customer.
GET /api/Customer/GetCustomer{id}: Get a Customer by ID.
POST /api/Customer/AddCustomer: Create a new Customer.
PUT /api/Customer/UpdateCustomer/{id}: Update an existing Customer.
DELETE /api/Customer/DeleteCustomer/{id}: Delete a Customer.

Orders
GET /api/Order/GetAllOrders: Get all Orders.
GET /api/Order/GetOrder{id}: Get a Order by ID.
POST /api/Order/AddOrder: Create a new Order.
PUT /api/Order/UpdateOrder/{id}: Update an existing Order.
DELETE /api/Order/DeleteOrder/{id}: Delete a Order.



DTOs Used
ProductCreateDTO: DTO for creating a new product.
ProductUpdateDTO: DTO for updating an existing product.
OrderCreateDTO: DTO for creating a new order.
OrderUpdateDTO: DTO for updating an existing order.

Models
Product: Represents a product with properties like Id, Name, Description, Price, and Stock.
Order: Represents an order with properties like Id, OrderDate, CustomerId, and OrderItems.
OrderItem: Represents an item within an order with properties like Id, OrderId, ProductId, Quantity, and UnitPrice.
Customer: Represents a customer with properties like Id, Name, and Email.
