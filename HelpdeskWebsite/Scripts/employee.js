$(function () { // employee.js
    const getAll = async (msg) => {
        try {
            $('#employeeList').html('Finding Employee Information, please wait...');
            let response = await fetch(`api/employees/`);
            if (!response.ok) // or check for response.status
                throw new Error(`Status - ${response.status}, Text - ${response.statusText}`);
            let data = await response.json(); // this returns a promise, so we await it
            buildEmployeeList(data);
            (msg === '') ? // are we appending to an existing message
                $('#status').text('Employees Loaded') : $('#status').text(`${msg} - employees Loaded`);
        } catch (error) {
            $('#status').text(error.message);
        }
    } // getAll

    const setupForUpdate = (Id, data) => {
        $('#actionbutton').val('update');
        $('#modaltitle').html('<h4>update employee</h4>');

        clearModalFields();
        data.map(employee => {
            if (employee.Id === parseInt(Id)) {
                $('#TextBoxTitle').val(employee.Title);
                $('#TextBoxFirstname').val(employee.Firstname);
                $('#TextBoxLastname').val(employee.Lastname);
                $('#TextBoxPhone').val(employee.Phoneno);
                $('#TextBoxEmail').val(employee.Email);
                localStorage.setItem('Id', employee.Id);
                localStorage.setItem('DepartmentId', employee.DepartmentId);
                localStorage.setItem('Timer', employee.Timer);
                $('#modalstatus').text('update data');
                $('#theModal').modal('toggle');
            } // if
        }); // data.map
    } // setupForUpdate

    const setupForAdd = () => {
        $('#actionbutton').val('add');
        $('#modaltitle').html('<h4>add employee</h4>');
        $('#theModal').modal('toggle');
        $('#modalstatus').text(' add new employee');
        clearModalFields();
    } // setupForAdd

    const clearModalFields = () => {
        $('#TextBoxTitle').val('');
        $('#TextBoxFirstname').val('');
        $('#TextBoxLastname').val('');
        $('#TextBoxPhone').val('');
        $('#TextBoxEmail').val('');
        localStorage.removeItem('Id');
        localStorage.removeItem('DepartmentId');
        localStorage.removeItem('Timer');
    } // clearModalFields

    //$('#employeeList').click((e) => {
    //    if (!e) e = window.event;
    //    let Id = e.target.parentNode.id;
    //    if (Id === 'employeeList' || Id === '') {
    //        Id = e.target.id;
    //    } // clicked on row somewhere else
    //    if (Id !== 'status' && Id !== 'heading') {
    //        let data = JSON.parse(localStorage.getItem('allemployees'));
    //        clearModalFields();
    //        data.map(employee => {
    //            if (employee.Id === parseInt(Id)) {
    //                $('#TextBoxTitle').val(employee.Title);
    //                $('#TextBoxFirstname').val(employee.Firstname);
    //                $('#TextBoxLastname').val(employee.Lastname);
    //                $('#TextBoxPhone').val(employee.Phoneno);
    //                $('#TextBoxEmail').val(employee.Email);
    //                localStorage.setItem('Id', employee.Id);
    //                localStorage.setItem('DepartmentId', employee.DepartmentId);
    //                localStorage.setItem('Timer', employee.Timer);
    //                $('#modalstatus').text('update data');
    //                $('#theModal').modal('toggle');
    //            } // if
    //        }); // data.map
    //    } else {
    //        return false; // ignore if they clicked on heading or status
    //    }
    //}); // employeeListClick

    //$('#updatebutton').click(async (e) => {
    //    try {
    //        emp = new Object();
    //        emp.Title = $('#TextBoxTitle').val();
    //        emp.Firstname = $('#TextBoxFirstname').val();
    //        emp.Lastname = $('#TextBoxLastname').val();
    //        emp.Phoneno = $('#TextBoxPhone').val();
    //        emp.Email = $('#TextBoxEmail').val();
    //        emp.Id = localStorage.getItem('Id');
    //        emp.DepartmentId = localStorage.getItem('DepartmentId');
    //        emp.Timer = localStorage.getItem('Timer');
    //        // send the updated back to the server asynchronously using PUT
    //        let response = await fetch('api/employees', {
    //            method: 'PUT',
    //            headers: {
    //                'Content-Type': 'application/json; charset=utf-8'
    //            },
    //            body: JSON.stringify(emp)
    //        });
    //        if (response.ok) // or check for response.status
    //        {
    //            let data = await response.json();  
    //            $('#status').text(data);
    //        } else {
    //            $('#status').text(`${response.status}, Text - ${response.statusText}`);
    //        } // else
    //        $('#theModal').modal('toggle');
    //    } catch (error) {
    //        $('#status').text(error.message);
    //    } // try/ catch

    //    //getAll(); // first grab the data from the server
    //}); // updatebutton click

    const buildEmployeeList = (data) => {
        $('#employeeList').empty();
        div = $(`<div class="list-group-item text-white bg-secondary row d-flex" id="status">Employee Info</div>
                    <div class= "list-group-item row d-flex text-center" id="heading">
                    <div class="col-4 h4">Title</div>
                    <div class="col-4 h4">First</div>
                    <div class="col-4 h4">Last</div>
                 </div>`);
        div.appendTo($('#employeeList'))
        localStorage.setItem('allemployees', JSON.stringify(data));
        btn = $(`<button class="list-group-item row d-flex" id="0"><div class="col-12 text-left">...click to add employee</div></button>`);
        btn.appendTo('#employeeList');
        data.map(emp => {
            btn = $(`<button class="list-group-item row d-flex" id="${emp.Id}">`);
            btn.html(`<div class="col-4" id="employeetitle${emp.Id}">${emp.Title}</div>
                      <div class="col-4" id="employeefname${emp.Id}">${emp.Firstname}</div>
                      <div class="col-4" id="employeelastname${emp.Id}">${emp.Lastname}</div>`
            );
            btn.appendTo($('#employeeList'));
        }); // map
    } // buildemployeeList

    const update = async () => {
        try {
            emp = new Object();
            emp.Title = $("#TextBoxTitle").val();
            emp.Firstname = $("#TextBoxFirstname").val();
            emp.Lastname = $("#TextBoxLastname").val();
            emp.Phoneno = $("#TextBoxPhone").val();
            emp.Email = $("#TextBoxEmail").val();
            emp.Id = localStorage.getItem("Id");
            emp.DepartmentId = localStorage.getItem("DepartmentId");
            emp.Timer = localStorage.getItem("Timer");
            // send the updated back to the server asynchronously using PUT
            let response = await fetch('api/employees', {
                method: 'PUT',
                headers: {
                    'Content-Type': 'application/json; charset=utf-8'
                },
                body: JSON.stringify(emp)
            });
            if (response.ok) // or check for response.status
            {
                let data = await response.json();
                getAll(data);
            } else {
                $('#status').text(`${response.status}; Text - ${response.statusText}`);
            } // else
            $('#theModal').modal('toggle');
        } catch (error) {
            $('#status').text(error.message);
        }
    } // update
    const add = async () => {
        try {
            emp = new Object();
            emp.Title = $("#TextBoxTitle").val();
            emp.Firstname = $("#TextBoxFirstname").val();
            emp.Lastname = $("#TextBoxLastname").val();
            emp.Phoneno = $("#TextBoxPhone").val();
            emp.Email = $("#TextBoxEmail").val();
            emp.DeparmentId = 100; // will add a dropdown later
            //emp.Id = -1;
            // send the employee info to the server asynchronously using POST
            let response = await fetch('api/employees', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json; charset=utf-8'
                },
                body: JSON.stringify(emp)
            });
            if (response.ok) // or check for response.status
            {
                let data = await response.json();
                getAll(data);
            } else {
                $('#status').text(`${response.status}, Text - ${response.statusText}`);
            } // else
            $('#theModal').modal('toggle');
        } catch (error) {
            $('#status').text(error.message);
        }
    } // add


    $("#actionbutton").click(() => {
        $("#actionbutton").val() === "update" ? update() : add();
    }); // actionbutton click

    $('#employeeList').click((e) => {
        if (!e) e = windown.event;
        let Id = e.target.parentNode.id;
        if (Id === 'employeeList' || Id === '') {
            Id = e.target.id;
        } // clicked on row somewhere else

        if (Id !== 'status' && Id !== 'heading') {
            let data = JSON.parse(localStorage.getItem('allemployees'));
            Id === '0' ? setupForAdd() : setupForUpdate(Id, data);
        } else {
            return false; // ignore if they clicked on heading or status
        }
    });

    getAll(''); // first grab the data from the server
}); // jQuery ready method