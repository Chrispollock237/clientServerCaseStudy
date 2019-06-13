// Emloyee JavaScript function that uses JQuery to load the employee list
// when the page gets loaded
$(function () { // employee.js

    // This getAll function goes to the server, retrieves the data and sotres the data
    // in the localStorage so we can use it. In this case to populate this List
    const getAll = async (msg) =>
    {
        try
        {
            // Prints a message onto the web page in the <dep id=employeeList> to let the user know
            // something is happening to retrieve the data they have requested.
            $('#employeeList').html('Finding Employee Information, please wait...');

            // The request to the server using the api/employees/ ROUTE in the StudentController
            // Grabs all the data from the request 
            let response = await fetch(`api/employees/`);

            // If the response is NOT ok, throw the Error
            if (!response.ok) // or check for response.status
                throw new Error(`Status - ${response.status}, Text - ${response.statusText}`);

            // Pulls the data out of the response object in the JSON format. It waits for 
            // this line of code to finish with the "await" key word, then goes to the 
            // the next line of code
            let data = await response.json(); // this returns a promise, so we await it

            // Calls the buildEmployeeList function to build and display the list of employees
            // with the data object we have created
            buildEmployeeList(data, true);

            // If the #status message is empty then we insert the 'Employees Loaded' message, but
            // if it is not empty then we appended to the message that is already in the #status dep
            (msg === '') ? // are we appending to an existing message
                $('#status').text('Employees Loaded') : $('#status').text(`${msg} - employees Loaded`);

            // Now response will wait for a call to the server using the api/departments/ ROUTE
            // in the DepartmentController to grab the department data
            response = await fetch(`api/departments/`);

            // If the response is NOT ok then throw an Error
            if (!response.ok) // or check for response.status
                throw new Error(`Status - ${response.status}, Text - ${response.statusText}`);

            // Pulls the data out of response. Waits for this line to complete
            // before continuing on to the next line
            let deps = await response.json(); // this returns a promise, so we await it

            // Stores the department data objects in the localStorage and converts them to
            // JSON format to be used later
            localStorage.setItem('alldepartments', JSON.stringify(deps));

        } catch (error) // If any errors are thrown, catches them and displays error message
        {
            $('#status').text(error.message);
        }
    } // getAll

    // filter the stored JSON base on srch contents
    const filterData = () => {
        allData = JSON.parse(localStorage.getItem('allemployees'));
        // tilde below same as stu.Lastname.indexOf($('#srch').val > -1)
        let filteredData = allData.filter((emp) => ~emp.Lastname.indexOf($('#srch').val()));
        buildEmployeeList(filteredData, false);
    } // filterData

    // Function pointer that sets up the modal for an employee update
    // Clears the modal of any previous data and bring up the employee data
    // that the user clicked on
    const setupForUpdate = (Id, data) =>
    {
        // Sets the button text to Update
        $('#actionbutton').val('Update');

        // Set the modal title
        $('#modaltitle').html('<h4>Update employee</h4>');

        // Clear the modal fields function so we don't append to the fields
        // if the modal has been called multiple times
        clearModalFields();

        // A loop to iterate through the data object to find the matching Id that
        // has been passed into the function, and populates the modal fields with 
        // the object that has the matching employeeId
        data.map(employee => {
            if (employee.Id === parseInt(Id)) {
                $('#TextBoxTitle').val(employee.Title);
                $('#TextBoxFirstname').val(employee.Firstname);
                $('#TextBoxLastname').val(employee.Lastname);
                $('#TextBoxPhone').val(employee.Phoneno);
                $('#TextBoxEmail').val(employee.Email);
                $('#ImageHolder').html(`<img height="120" width="110" src="data:image/png;base64,${employee.StaffPicture64}"/>`);

                // Calls the drop down list function to populate the drop down list
                loadDepDDLValue(employee.DepartmentId);

                // Sets the Id, the Employee object timer, and the Staff Picture into the local storage
                // to be used later to check for concurrency
                localStorage.setItem('Id', employee.Id);
                localStorage.setItem('Timer', employee.Timer);
                localStorage.setItem('StaffPicture64', employee.StaffPicture64);

                
                $('#modalstatus').text('update data');
                $('#theModal').modal('toggle');
            } // if
        }); // data.map
    } // setupForUpdate

    // setupForAdd() function. Toggle to modal and sets the button text(value) to "Add"
    const setupForAdd = () =>
    {
        // Sets the '#actionbutton' text(value) to Add
        $('#actionbutton').val('Add');

        // Fase out the Delete button because you can't delete someone before they are
        // in the database
        $('#deletebutton').fadeOut();

        // Modal header and toggle the modal to show
        $('#modaltitle').html('<h4>Add employee</h4>');
        $('#theModal').modal('toggle');

        // Status on the modal saying to add new employee
        $('#modalstatus').text('add new employee');

        // The employeeId is -1  so there is no value in the DropDownList initially
        // and must be chosen by the user
        loadDepDDLValue(-1);

        // Clears all the Modal fields to make sure they are all empty so the user 
        // can input all their own data
        clearModalFields();

    } // setupForAdd

    // Function to load the departments in a DropDownList
    const loadDepartmentDDL = async () =>
    {
        // Set Departments, waits for the "fetch" call to finish collecting the data
        // from the departments ROUTE in the DepartmentController
        let response = await fetch(`api/departments/`);

        // If the response is NOT ok than throw an Error
        if (!response.ok) // or check for response.status
            throw new Error(`Status - ${response.status}, Text - ${response.statusText}`);

        // Holds all of the deparment objects the you just retrieved from the database
        // in JSON format
        let deps = await response.json(); // this returns a promise, so we await it

        // Create drop down list
        // HTML is empty, and it empties out the DropDownList to make sure it doesn't 
        // keep appending to the list having multiples of all the departments
        html = '';
        $('#ddlDeps').empty();

        // Loop to iterate through all the department objects and appends them to the 
        // DropDownList
        deps.map(dep => html += `<option value="${dep.Id}">${dep.Name}</option>`);
        $('#ddlDeps').append(html);
        
    } // loadDepartmentDDL

    // Loads the values (prevents calling loadDepartmentDDL multiple times without need)
    // Because no one likes wasting cycles
    const loadDepDDLValue = (empDepId) =>
    {
        // sets the selected option right here
        $('#ddlDeps').val(empDepId)
    }

    // clearModalFields() function to empty all of the modal fields so the user does NOT
    // have to clear or backspace the data everytime the modal pops up
    const clearModalFields = () => {
        $('#TextBoxTitle').val('');
        $('#TextBoxFirstname').val('');
        $('#TextBoxLastname').val('');
        $('#TextBoxPhone').val('');
        $('#TextBoxEmail').val('');
        loadDepDDLValue(-1);

        localStorage.removeItem('Id');
        localStorage.removeItem('DepartmentId');
        localStorage.removeItem('StaffPicture64');
        localStorage.removeItem('Timer');
    } // clearModalFields

    // buildEmployeeList() function to create the list-group for the user to see all
    // of the employees and either add an employee to the list or update a current employee 
    const buildEmployeeList = (data, allemployees) =>
    {
        // Makes sure the list is empty everytime we call it so we don't have
        // an appended list with the list copied many times over
        $('#employeeList').empty();

        // Creates the HTML deps and adds the title headings to the columns
        div = $(`<div class="list-group-item text-white bg-primary row d-flex justify-content-center" id="status">Employee Info</div>
                    <div class= "list-group-item row d-flex bg-default text-center" id="heading">
                    <div class="col-4 h4">Title</div>
                    <div class="col-4 h4">First</div>
                    <div class="col-4 h4">Last</div>
                 </div>`);

        // Adds the headings to the employeeList
        div.appendTo($('#employeeList'))

        // Week 10 Notes
        allemployees ? localStorage.setItem('allemployees', JSON.stringify(data)) : null;

        // Stores all the JavaScript data objects in the localStorage and converts
        // the objects into JSON format so we can access the data later
        //localStorage.setItem('allemployees', JSON.stringify(data));

        // We add one more row which is essentially a button that allows the users to an Employee
        // to the list, then it appends the row(button) to the employeeList
        btn = $(`<button class="list-group-item row d-flex text-dark btn-hover btn-info" id="0"><dep class="col-12 justify-text-center">...Click to add employee</dep></button>`);
        btn.appendTo('#employeeList');

        // A loop that iterates through all the data objects we have "fetched" from the database
        // and adds a row on to the employeeList. Each row is a clickable button the pops a modal
        // up to further interact with the employee object
        data.map(emp => {

            // Create the container for each button and set the ID for the employee as the ID
            // for the container to be used for the UPDATE, ADD, and DELETE functions later
            btn = $(`<button class="list-group-item row d-flex text-dark btn-hover btn-info" id="${emp.Id}">`);
            btn.html(`<div class="col-4" id="employeetitle${emp.Id}">${emp.Title}</div>
                      <div class="col-4" id="employeefname${emp.Id}">${emp.Firstname}</div>
                      <div class="col-4" id="employeelastname${emp.Id}">${emp.Lastname}</div>`
            );
            btn.appendTo($('#employeeList'));
        }); // map
    } // buildemployeeList

    // function to update the employee data asynchronously 
    const update = async () =>
    {
        try
        {
            // Create a new employee object and grab the values from the TextBoxes
            // and get the employee department name from the drop down list
            emp = new Object();
            emp.Title = $("#TextBoxTitle").val();
            emp.Firstname = $("#TextBoxFirstname").val();
            emp.Lastname = $("#TextBoxLastname").val();
            emp.Phoneno = $("#TextBoxPhone").val();
            emp.Email = $("#TextBoxEmail").val();
            emp.Id = localStorage.getItem("Id");
            emp.DepartmentId = $('#ddlDeps').val();
            localStorage.getItem('StaffPicture64') ? emp.StaffPicture64 = localStorage.getItem('StaffPicture64') : null;
            emp.Timer = localStorage.getItem("Timer");

            // Send the updated employee object back to the server asynchronously using PUT method
            // in the EmployeeController. The await key word waits for the 'PUT' method to
            // before moving onto the next line of code
            let response = await fetch('api/employees', {
                method: 'PUT',
                headers: {
                    'Content-Type': 'application/json; charset=utf-8'
                },
                body: JSON.stringify(emp)
            });

            // If the response to the server is "ok" then the employee data is put back
            // into an employee object so that the list can be populated again with the
            // updated information
            if (response.ok) // or check for response.status
            {
                // Holds all of the employee objects data that you just sent to the database
                // in JSON format
                let data = await response.json();

                // Gets all the data and creates the updated list with the employee
                getAll(data);

            } else
            {
                // Else the response is NOT "Ok" and the status and message of why it was not "Ok"
                $('#status').text(`${response.status}; Text - ${response.statusText}`);
            } // else

            // Toggle the modal
            $('#theModal').modal('toggle');

        } catch (error) // if there was an error, catch it and display the error message
        {
            $('#status').text(error.message);
        }
    } // update

    // function to add a new employee to the database asynchronously 
    const add = async () => {
        try {
            emp = new Object();
            emp.Title = $("#TextBoxTitle").val();
            emp.Firstname = $("#TextBoxFirstname").val();
            emp.Lastname = $("#TextBoxLastname").val();
            emp.Phoneno = $("#TextBoxPhone").val();
            emp.Email = $("#TextBoxEmail").val();
            emp.DepartmentId = $("#ddlDeps").val();
            emp.StaffPicture64 = localStorage.getItem('StaffPicture64');
            
            // send the employee info to the server asynchronously using POST method from 
            // the EmployeeController. The await key word waits for the 'POST' method to
            // before moving onto the next line of code
            let response = await fetch('api/employees', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json; charset=utf-8'
                },
                body: JSON.stringify(emp)
            });

            // If the response to the server is "ok" then the employee data is put
            // into an employee object so that the list can be populated with the
            // the new employee data
            if (response.ok) // or check for response.status
            {
                // Holds all of the employee objects data that you just sent to the database
                // in JSON format
                let data = await response.json();

                // Gets all the data and creates the updated list with the employee
                // with the new employee
                getAll(data);
            }
            else
            {
                 // Else the response is NOT "Ok" and the status and message of why it was not "Ok"
                $('#status').text(`${response.status}, Text - ${response.statusText}`);
            } // else

            // Toggle the modal
            $('#theModal').modal('toggle');

        } catch (error) // if there was an error, catch it and display the error message
        {
            $('#status').text(error.message);
        }
    } // add

    // function to delete an employee in the database asynchronously 
    let _delete = async () =>
    {
        try
        {
            // sends a call to the database to 'DELETE' an employee using the 'DELETE' method
            // from the EmployeeController and awaits the "Promise" before continuing to the next line of code
            let response = await fetch(`api/departments/${localStorage.getItem('Id')}`, {
                method: 'DELETE',
                headers: {
                    'Content-Type': 'application/json; charset=utf-8'
                }
            });

            // If the response to the server is "ok" then the employee data is deleted
            // and then the data is put into an employee object so that the list can be
            // populated with the employee data that is still in the database
            if (response.ok) // or check for response.status
            {
                // Holds all of the employee objects data that you just sent to the database
                // in JSON format
                let data = await response.json();

                // Gets all the data and creates the updated list with the employees
                // that are still in the database
                getAll(data);
            }
            else
            {
                // Else the response is NOT "Ok" and the status and message of why it was not "Ok"
                $('#status').text(`${response.status}, Text - ${response.statusText}`);
            } // else

            // Toggle the modal
            $('#theModal').modal('toggle');

        } catch (error)  // if there was an error, catch it and display the error message
        {
            $('#status').text(error.message);
        }
    }

    $("#EmployeeModalForm").validate({
        rules: {
            TextBoxTitle: { maxlength: 4, required: true, validTitle: true },
            TextBoxFirstname: { maxlength: 25, required: true },
            TextBoxLastname: { maxlength: 25, required: true },
            TextBoxEmail: { maxlength: 40, required: true, email: true },
            TextBoxPhone: { maxlength: 15, required: true }
        },
        errorElement: "div",
        messages: {
            TextBoxTitle: {
                required: "required 1-4 chars.", maxlength: "required 1-4 chars.", validTitle: "Mr. Ms. Mrs. or Dr."
            },
            TextBoxFirstname: {
                required: "required 1-25 chars.", maxlength: "required 1-25 chars."
            },
            TextBoxLastname: {
                required: "required 1-25 chars.", maxlength: "required 1-25 chars."
            },
            TextBoxPhone: {
                required: "required 1-15 chars.", maxlength: "required 1-15 chars."
            },
            TextBoxEmail: {
                required: "required 1-40 chars.", maxlength: "required 1-40 chars.", email: "Please enter vaild email format (e.g. abc@abc.com)"
            }
        }
    });

    $.validator.addMethod("validTitle", function (value, element) { // custom rule
        return this.optional(element) || (value == "Mr." || value == "Ms." || value == "Mrs." || value == "Dr.");
    }, "");


    // The click event handler for the button on the modal. If the value(text) of
    // the button is "Update" then the update() function is called. If the value(text)
    // is not "Update" then the add() funcion is called.
    $("#actionbutton").click((e) => {
        $('#modalstatus').removeClass();
        if ($("#EmployeeModalForm").valid()) {

            $("#modalstatus").attr('class', 'badge badge-success');
            $('#modalstatus').text('Data Validated by jQuery!');
            $("#actionbutton").val() === "Update" ? update() : add();

        } else {

            $("#modalstatus").attr('class', 'badge badge-danger');
            $("#modalstatus").text("Please Fix Errors");
            e.preventDefault;
        }
        return false; // ignore click so modal remains
    }); // actionbutton click

    // A confirmation "Yes / No" option appears on the modal when the delete button
    // is clicked to make a confirmation that this is the button the user mean to click
    $('[data-toggle=confirmation]').confirmation({ rootSelector: '[data-toggle=confirmation]' });
    $('#deletebutton').click(() => _delete()); // if yes was chosen

    // When you click on the employeeList it pulls the ID out of the employee that has been 
    // clicked on. 
    $('#employeeList').click((e) => {

        if (!e) e = windown.event;

        // ID is assigned the ID of the row(button) that has been clicked on. If clicked on
        // an Employee, then the ID will be that of the Employees ID
        let Id = e.target.parentNode.id;

        // If the ID of the object that has been clicked in the html is that of the
        // or there is no current ID of the object clicked like on one of the rows
        // then Id is assigned to where the user clicked
        if (Id === 'employeeList' || Id === '')
        {
            Id = e.target.id;
        } // clicked on row somewhere else

        // If the ID does NOT equal status or heading then we are going to use that ID
        // to ADD, DELETE, or UPDATE
        if (Id !== 'status' && Id !== 'heading') {

            // Retrieving allemployees data from the localStorage. Parse the object 
            // to get all of the information from that object (i.e. Title, Firstname)
            let data = JSON.parse(localStorage.getItem('allemployees'));

            // Id is assigned the Id of that row(button). If the Id is 0 than call the
            // setupAdd() function, and if it is NOT 0 call the setupForUpdate() function.
            // Past the setupForUpdate() function the Id that has been clicked on, and the 
            // data from the row(button)
            Id === '0' ? setupForAdd() : setupForUpdate(Id, data);
        }
        else
        {
            return false; // ignore if they clicked on heading or status
        }
    });

    // do we have a picture?
    $('input:file').change(() => {
        const reader = new FileReader();
        const file = $('#fileUpload')[0].files[0];

        file ? reader.readAsBinaryString(file) : null;

        reader.onload = (readerEvt) => {
            // get binary data then convert to encoded string
            const binaryString = reader.result;
            const encodedString = btoa(binaryString);
            localStorage.setItem('StaffPicture64', encodedString);
        }
    }); // input file change

    $('#srch').keyup(filterData); // srch key press

    // Calls the getAll function to grab the data from the server
    getAll(''); // first grab the data from the server

    // Grab the departments and add them to the Drop Down List by calling
    // the loadDepartmentDDL() function
    loadDepartmentDDL(); 
}); // jQuery ready method