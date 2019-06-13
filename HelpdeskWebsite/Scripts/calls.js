// Emloyee JavaScript function that uses JQuery to load the employee list
// when the page gets loaded
$(function () { // employee.js

    // This getAll function goes to the server, retrieves the data and sotres the data
    // in the localStorage so we can use it. In this case to populate this List
    const getAll = async (msg) => {
        try {
            // Prints a message onto the web page in the <div id=callList> to let the user know
            // something is happening to retrieve the data they have requested.
            $('#callList').html('Finding Call Information, please wait...');

            // The request to the server using the api/calls/ ROUTE in the CallController
            // Grabs all the data from the request 
            let response = await fetch(`api/calls`);

            // If the response is NOT ok, throw the Error
            if (!response.ok) // or check for response.status
                throw new Error(`Status - ${response.status}, Text - ${response.statusText}`);

            // Pulls the data out of the response object in the JSON format. It waits for 
            // this line of code to finish with the "await" key word, then goes to the 
            // the next line of code
            let data = await response.json(); // this returns a promise, so we await it

            // Calls the buildCallList function to build and display the list of employees
            // with the data object we have created
            buildCallList(data, true);

            // If the #status message is empty then we insert the 'Employees Loaded' message, but
            // if it is not empty then we appended to the message that is already in the #status dep
            (msg === '') ? // are we appending to an existing message
                $('#status').text('Calls Loaded') : $('#status').text(`${msg} - Calls Loaded`);

        } catch (error) // If any errors are thrown, catches them and displays error message
        {
            $('#status').text(error.message);
        }
    } // getAll

    // filter the stored JSON based on search contents
    const filterData = () => {
        allData = JSON.parse(localStorage.getItem('allcalls'));
        // tilde below same as stu.Lastname.indexOf($('#srch').val > -1)
        let filteredData = allData.filter((emp) => ~emp.EmployeeName.indexOf($('#srch').val()));
        buildCallList(filteredData, false);
    } // filterData

    // Function to format the date for when the call was opened/ closed
    const formatDate = (data) => {
        let d;
        (data === undefined) ? d = new Date() : d = new Date(Date.parse(data));
        let _day = d.getDate();
        let _month = d.getMonth() + 1;
        let _year = d.getFullYear();
        let _hour = d.getHours();
        let _min = d.getMinutes();
        if (_min < 10) { _min = "0" + _min; }
        return _year + "-" + _month + "-" + _day + " " + _hour + ":" + _min;

    } // formatDate

    // Function pointer that sets up the modal for a call update
    // Clears the modal of any previous data and bring up the employee data
    // that the user clicked on
    const setupForUpdate = (Id, data) => {
        // Sets the button text to Update
        $('#actionbutton').val('Update');

        // Set the modal title
        $('#modaltitle').html('<h4>Update Call</h4>');

        // Clear the modal fields function so we don't append to the fields
        // if the modal has been called multiple times
        clearModalFields();

        // A loop to iterate through the data object to find the matching Id that
        // has been passed into the function, and populates the modal fields with 
        // the object that has the matching employeeId
        data.map(call => {
            if (call.Id === parseInt(Id))
            {
                if (!call.OpenStatus)
                {
                    // Hide the action(Update) button and disable all fields if the Call is closed
                    $('#actionbutton').fadeOut();
                    $('#employeeDDL').prop('disabled', true);
                    $('#technicianDDL').prop('disabled', true);
                    $('#problemDDL').prop('disabled', true);
                    $('#TextBoxNotes').prop('disabled', true);
                    $('#closeCall').prop('disabled', true);
                    $('#closeCall').prop('checked', true);
                    $('#labelDateClosed').text(formatDate(call.DateClosed));
                    $('#dateClosed').val(formatDate(call.DateClosed));

                    $('#deletebutton').fadeIn();

                    // Calls the drop down list function to populate the drop down list
                    loadProblemDDL(call.ProblemId);
                    loadEmployeeDDL(call.EmployeeId);
                    loadTechnicianDDL(call.TechId);
                    $('#labelDateOpened').text(formatDate(call.DateOpened));
                    $('#dateOpened').val(formatDate(call.DateOpened));
                    $('#TextBoxNotes').val(call.Notes);

                    // Make sure to store the Id so they can still use the Id to delete the call if they want later
                    localStorage.setItem('Id', call.Id);

                    $('#modalstatus').text('Can Not Update Call, Call Is Closed');
                    $('#theModal').modal('toggle');
                }
                else
                {
                    // If the action button had been disabled in another call, make sure to add it back in
                    $('#actionbutton').fadeIn();

                    // If fields have been disabled previously make sure to enable them again
                    $('#employeeDDL').prop('disabled', false);
                    $('#technicianDDL').prop('disabled', false);
                    $('#problemDDL').prop('disabled', false);
                    $('#TextBoxNotes').prop('disabled', false);
                    $('#closeCall').prop('disabled', false);
                    $('#closeCall').prop('checked', false);

                    $('#deletebutton').fadeIn();

                    // Calls the drop down list function to populate the drop down list
                    loadProblemDDL(call.ProblemId);
                    loadEmployeeDDL(call.EmployeeId);
                    loadTechnicianDDL(call.TechId);
                    $('#labelDateOpened').text(formatDate(call.DateOpened));
                    $('#dateOpened').val(formatDate(call.DateOpened));
                    $('#TextBoxNotes').val(call.Notes);

                    // Sets the Id, and the Call object timer into the local storage
                    // to be used later to check for concurrency
                    localStorage.setItem('Id', call.Id);
                    localStorage.setItem('Timer', call.Timer);

                    $('#modalstatus').text('Update Call');
                    $('#theModal').modal('toggle');
                }
            } // if
        }); // data.map

        
    } // setupForUpdate

    // setupForAdd() function. Toggle to modal and sets the button text(value) to "Add"
    const setupForAdd = async () => {
        // Sets the '#actionbutton' text(value) to Add
        $('#actionbutton').val('Add');
        $('#actionbutton').fadeIn();

        clearModalFields();

        // If fields have been disabled previously make sure to enable them again
        $('#employeeDDL').prop('disabled', false);
        $('#technicianDDL').prop('disabled', false);
        $('#problemDDL').prop('disabled', false);
        $('#TextBoxNotes').prop('disabled', false);
        $('#closeCall').prop('disabled', false);
        $('#closeCall').prop('checked', false);

        //hide the delete button, as you won't be deleting a call as you add it
        $('#deletebutton').fadeOut();

        // Modal header and toggle the modal to show
        $('#modaltitle').html('<h4>Add Call</h4>');
        $('#theModal').modal('toggle');

        // Status on the modal saying to add new employee
        $('#modalstatus').text('Add New Call');

        // The employeeId is -1  so there is no value in the DropDownList initially
        // and must be chosen by the user
        loadProblemDDL(-1);
        loadEmployeeDDL(-1);
        loadTechnicianDDL(-1);

        let response = await fetch(`api/calls`);

        // If the response is NOT ok than throw an Error
        if (!response.ok) // or check for response.status
            throw new Error(`Status - ${response.status}, Text - ${response.statusText}`);

        let call = await response.json();

        $('#labelDateOpened').text(formatDate(call.DateOpened));
        $('#dateOpened').val(formatDate(call.DateOpened));

        // Clears all the Modal fields to make sure they are all empty so the user 
        // can input all their own data
        //clearModalFields();

    } // setupForAdd

    // Function to load the problems in a DropDownList
    const loadProblemDDL = async (problemId) => {
        // Set Responsess, waits for the "fetch" call to finish collecting the data
        // from the departments ROUTE in the DepartmentController
        let response = await fetch(`api/problems`);

        // If the response is NOT ok than throw an Error
        if (!response.ok) // or check for response.status
            throw new Error(`Status - ${response.status}, Text - ${response.statusText}`);

        // Holds all of the problems objects the you just retrieved from the database
        // in JSON format
        let probs = await response.json(); // this returns a promise, so we await it

        // Create drop down list
        // HTML is empty, and it empties out the DropDownList to make sure it doesn't 
        // keep appending to the list having multiples of all the departments
        html = '';
        $('#problemDDL').empty();

        // Loop to iterate through all the department objects and appends them to the 
        // DropDownList
        probs.map(probs => html += `<option value="${probs.Id}">${probs.Description}</option>`);
        $('#problemDDL').append(html);

        if (problemId == null)
            $('#problemDDL').val(-1);
        else
            $('#problemDDL').val(problemId);
        
    } // loadProblemDDL

    // Function to load the technicians in a DropDownList
    const loadTechnicianDDL = async (techId) => {
        // Set Technicians, waits for the "fetch" call to finish collecting the data
        // from the employees ROUTE in the EmployeeController
        let response = await fetch(`api/employees/`);

        // If the response is NOT ok than throw an Error
        if (!response.ok) // or check for response.status
            throw new Error(`Status - ${response.status}, Text - ${response.statusText}`);

        // Holds all of the technician objects the you just retrieved from the database
        // in JSON format
        let technician = await response.json(); // this returns a promise, so we await it

        // Create drop down list
        // HTML is empty, and it empties out the DropDownList to make sure it doesn't 
        // keep appending to the list having multiples of all the departments
        html = '';
        $('#technicianDDL').empty();

        // Loop to iterate through all the employee objects and appends them to the 
        // DropDownList
        technician.map(tech => {
            if (tech.IsTech == 1)
                html += `<option value="${tech.Id}">${tech.Lastname}</option>`
        });
        $('#technicianDDL').append(html);

        if (techId == null)
            $('#technicianDDL').val(-1);
        else
            $('#technicianDDL').val(techId);

    } // loadtechnicianDDL

    // Function to load the problems in a DropDownList
    const loadEmployeeDDL = async (employeeId) => {
        // Set Employees, waits for the "fetch" call to finish collecting the data
        // from the employees ROUTE in the EmployeeController
        let response = await fetch(`api/employees/`);

        // If the response is NOT ok than throw an Error
        if (!response.ok) // or check for response.status
            throw new Error(`Status - ${response.status}, Text - ${response.statusText}`);

        // Holds all of the employee objects the you just retrieved from the database
        // in JSON format
        let emps = await response.json(); // this returns a promise, so we await it

        // Create drop down list
        // HTML is empty, and it empties out the DropDownList to make sure it doesn't 
        // keep appending to the list having multiples of all the departments
        html = '';
        $('#employeeDDL').empty();

        // Loop to iterate through all the employee objects and appends them to the 
        // DropDownList
        emps.map(emp => html += `<option value="${emp.Id}">${emp.Lastname}</option>`);
        $('#employeeDDL').append(html);

        if (employeeId == null)
            $('#employeeDDL').val(-1);
        else
            $('#employeeDDL').val(employeeId);
    } // loadProblemDDL

    // clearModalFields() function to empty all of the modal fields so the user does NOT
    // have to clear or backspace the data everytime the modal pops up
    const clearModalFields = () => {
        $('#TextBoxNotes').val('');
        $('#labelDateClosed').text('');
        $('#dateClosed').val('');
        loadProblemDDL(-1);
        loadEmployeeDDL(-1);
        loadTechnicianDDL(-1);
        localStorage.removeItem('Id');
        localStorage.removeItem('Timer');
    } // clearModalFields

    // buildEmployeeList() function to create the list-group for the user to see all
    // of the employees and either add an employee to the list or update a current employee 
    const buildCallList = (data, allcalls) => {
        // Makes sure the list is empty everytime we call it so we don't have
        // an appended list with the list copied many times over
        $('#callList').empty();

        // Creates the HTML deps and adds the title headings to the columns
        div = $(`<div class="list-group-item text-white bg-primary row d-flex justify-content-center" id="status">Call Info</div>
                    <div class= "list-group-item row d-flex bg-default text-center" id="heading">
                    <div class="col-4 h4">Date</div>
                    <div class="col-4 h4">Name</div>
                    <div class="col-4 h4">Problem</div>
                 </div>`);

        // Adds the headings to the callList
        div.appendTo($('#callList'))

        // Week 10 Notes
        allcalls ? localStorage.setItem('allcalls', JSON.stringify(data)) : null;

        // Stores all the JavaScript data objects in the localStorage and converts
        // the objects into JSON format so we can access the data later
        //localStorage.setItem('allemployees', JSON.stringify(data));

        // We add one more row which is essentially a button that allows the users to add a Call
        // to the list, then it appends the row(button) to the callList
        btn = $(`<button class="list-group-item row d-flex text-dark btn-hover btn-info" id="0"><div class="col-12 justify-text-center">...Click to Add Call</div></button>`);
        btn.appendTo('#callList');

        // A loop that iterates through all the data objects we have "fetched" from the database
        // and adds a row on to the employeeList. Each row is a clickable button the pops a modal
        // up to further interact with the employee object
        data.map(call => {

            // Create the container for each button and set the ID for the employee as the ID
            // for the container to be used for the UPDATE, ADD, and DELETE functions later
            btn = $(`<button class="list-group-item row d-flex text-dark btn-hover btn-info" id="${call.Id}">`);
            btn.html(`<div class="col-4" id="call${call.Id}">${formatDate(call.DateOpened)}</div>
                      <div class="col-4" id="callLastname${call.Id}">${call.EmployeeName}</div>
                      <div class="col-4" id="employeelastname${call.Id}">${call.ProblemDescription}</div>`
            );
            btn.appendTo($('#callList'));
        }); // map
    } // buildcallList

    // function to update the employee data asynchronously 
    const update = async () => {
        try {
            // Create a new employee object and grab the values from the TextBoxes
            // and get the employee department name from the drop down list
            call = new Object();
            call.EmployeeId = $('#employeeDDL').val();
            call.EmployeeName = $('#employeeDDL').val();
            call.TechId = $('#technicianDDL').val();
            call.ProblemId = $('#problemDDL').val();
            call.DateOpened = $('#dateOpened').val();
            call.DateClosed = $('#dateClosed').val();
            call.Notes = $('#TextBoxNotes').val();           
            call.Timer = localStorage.getItem('Timer');
            call.Id = localStorage.getItem('Id');

            if ($('#closeCall').is(':checked'))
            {
                call.OpenStatus = false;
            }
            else
            {
                call.OpenStatus = true;
            }
           
            // Send the updated employee object back to the server asynchronously using PUT method
            // in the EmployeeController. The await key word waits for the 'PUT' method to
            // before moving onto the next line of code
            let response = await fetch('api/calls', {
                method: 'PUT',
                headers: {
                    'Content-Type': 'application/json; charset=utf-8'
                },
                body: JSON.stringify(call)
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

            } else {
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

    // function to add a new call to the database asynchronously 
    const add = async () => {
        try {
            call = new Object();
            call.EmployeeId = $('#employeeDDL').val();
            call.EmployeeName = $('#employeeDDL').val();
            call.TechId = $('#technicianDDL').val();
            call.ProblemId = $('#problemDDL').val();
            call.DateOpened = $('#dateOpened').val();
            call.OpenStatus = true;
            call.Notes = $('#TextBoxNotes').val();
       
            // send the employee info to the server asynchronously using POST method from 
            // the EmployeeController. The await key word waits for the 'POST' method to
            // before moving onto the next line of code
            let response = await fetch('api/calls', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json; charset=utf-8'
                },
                body: JSON.stringify(call)
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
            else {
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
    let _delete = async () => {
        try {
            // sends a call to the database to 'DELETE' an employee using the 'DELETE' method
            // from the EmployeeController and awaits the "Promise" before continuing to the next line of code
            let response = await fetch(`api/calls/${localStorage.getItem('Id')}`, {
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
            else {
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

    $("#CallModalForm").validate({
        rules: {
            problemDDL: { required: true/*, validProblem: true*/ },
            employeeDDL: { required: true },
            technicianDDL: { required: true },
            TextBoxNotes: { maxlength: 250, required: true }
        },
        errorElement: "div",
        messages: {
            problemDDL: {
                required: "Select a Problem."
            },
            employeeDDL: {
                required: "Select an Employee with the problem."
            },
            technicianDDL: {
                required: "Select a Technician to help with the problem."
            },
            TextBoxNotes: {
                required: "Please leave a note.", maxlength: "Required 1-250 characters."
            }
        }
    });

    // Click event handler for the check box to say whether or not the call has been closed
    $('#closeCall').click(() => {
        if ($('#closeCall').is(':checked')) {
            $('#labelDateClosed').text(formatDate());
            $('#dateClosed').val(formatDate());
        }
        else {
            $('#labelDateClosed').text('');
            $('#dateClosed').val('');
        }
    }); // checkBoxClose

    // The click event handler for the button on the modal. If the value(text) of
    // the button is "Update" then the update() function is called. If the value(text)
    // is not "Update" then the add() funcion is called.
    $("#actionbutton").click((e) =>
    {
        $('#modalstatus').removeClass();

        if ($("#CallModalForm").valid())
        {
            $("#modalstatus").attr('class', 'badge badge-success');
            $('#modalstatus').text('Data Validated by jQuery!');
            $("#actionbutton").val() === "Update" ? update() : add();
        }
        else
        {
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
    $('#callList').click((e) => {

        if (!e) e = windown.event;

        // ID is assigned the ID of the row(button) that has been clicked on. If clicked on
        // an Employee, then the ID will be that of the Employees ID
        let Id = e.target.parentNode.id;

        // If the ID of the object that has been clicked in the html is that of the
        // or there is no current ID of the object clicked like on one of the rows
        // then Id is assigned to where the user clicked
        if (Id === 'callList' || Id === '') {
            Id = e.target.id;
        } // clicked on row somewhere else

        // If the ID does NOT equal status or heading then we are going to use that ID
        // to ADD, DELETE, or UPDATE
        if (Id !== 'status' && Id !== 'heading') {

            // Retrieving allemployees data from the localStorage. Parse the object 
            // to get all of the information from that object (i.e. Title, Firstname)
            let data = JSON.parse(localStorage.getItem('allcalls'));

            // Id is assigned the Id of that row(button). If the Id is 0 than call the
            // setupAdd() function, and if it is NOT 0 call the setupForUpdate() function.
            // Past the setupForUpdate() function the Id that has been clicked on, and the 
            // data from the row(button)
            Id === '0' ? setupForAdd() : setupForUpdate(Id, data);
        }
        else {
            return false; // ignore if they clicked on heading or status
        }
    });

    $('#srch').keyup(filterData); // srch key press

    // Calls the getAll function to grab the data from the server
    getAll(''); // first grab the data from the server

    // Grab the departments and add them to the Drop Down List by calling
    // the loadProblemDDL() function
    loadProblemDDL();
    loadEmployeeDDL();
    loadTechnicianDDL();
}); // jQuery ready method