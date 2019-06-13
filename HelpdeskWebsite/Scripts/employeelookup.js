/*
    Name:           employeelookup.js       
    Author:         Chris Pollock
    Date:           October 26, 2018
    Purpose:        JavaScript loaded when the page is loaded to lookup an employee in the database
 */
$(function () {

    $('#getbutton').mouseup(async (e) => { // click event handler makes asynchronous fetch to server

        try {
            let lastname = $('#TextBoxLastname').val();
            $('#status').text('please wait...');
            let response = await fetch(`api/employees/${lastname}`);
            if (!response.ok) // or check for response.status
                throw new Error(`Status - ${response.status}, Text - ${response.statusText}`);
            let data = await response.json(); // this returns a promise, so we await it
            if (data.Lastname !== 'not found') {
                $('#email').text(data.Email);
                $('#title').text(data.Title);
                $('#firstname').text(data.Firstname);
                $('#phone').text(data.Phoneno);
                $('#status').text('employee found');
            }
            else {
                $('#firstname').text('not found');
                $('#email').text('');
                $('#title').text('');
                $('#phone').text('');
                $('#status').text('no such employee');
            }
        }
        catch (error) {
            $('status').text(error.message);
        } // try/ catch
    }); // mouse event
}); // jQuery ready method