# ProductCRUDs 
Here are the project requirements summarized in bullet points:

**Project Objective:**
- Develop a web application using ASP.NET Core to implement CRUD (Create, Read, Update, Delete) operations for a product management system. 

**Timeframe:**
- Complete the project within two days.

**Submission:**
- Upload the project code to GitHub.  

**API Requirements:**
1. **Authentication API:**
   - Endpoint: https://stg-zero.propertyproplus.com.au/api/TokenAuth/Authenticate
   - Include the Tenant ID (Abp.TenantId) in the header with the value 10.
   - Send a POST request with a JSON request body containing the username and password.
   - Expect an access token in the response.

2. **Get All Product API (GET):**
   - Endpoint: https://stg-zero.propertyproplus.com.au/api/services/app/ProductSync/GetAllproduct
   - Include the access token from the authentication API response in the Authorization header (Bearer token).
   - Expect a JSON response containing a list of products.

3. **Create and Update API (PATCH/POST):**
   - Endpoint: https://stg-zero.propertyproplus.com.au/api/services/app/ProductSync/CreateOrEdit
   - Include the access token from the authentication API response in the Authorization header (Bearer token).
   - Send a POST or PATCH request with a JSON request body containing product details for creation or updating.

**CRUD Operations:**
- Implement the following CRUD operations for products:
   1. Login.
   2. Get Product (Read).
   3. Create Product (Create).
   4. Update Product (Update).
 
