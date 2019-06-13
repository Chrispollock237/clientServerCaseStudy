$(function () {

    // On click event for the Call report pdf button
    $('#callPdf').click(async (e) => {
        try {
            // Display a message to the user to let them know something is happening
            $('#lblstatus').text('Generating report on server - please wait...');

            // Fetches the data from the database for all the calls
            let response = await fetch('api/callreport');

            if (!response.ok) // check for response status
                throw new Error(`Status - ${response.status}, Text - ${response.statusText}`);

            // All call data is then returned in JSON format and if the report was successfully generated then open the PDF 
            // in the new window
            let data = await response.json(); // this returns a promise, so we await it
            (data === 'Report Generated') ? window.open('/Pdfs/Calls.pdf') : $('#lblstatus').text('problem generating report');

            // Let user know the report has been successfully generated
            if (data === 'Report Generated')
                $('#lblstatus').text('Report Generated');
        }
        catch (error) {
            $('#lblstatus').text(error.message);
        } // try catch
    }); // call button click

    $('#employeePdf').click(async (e) => {
        try {
            // Display a message to the user to let them know something is happening
            $('#lblstatus').text('Generating report on server - please wait...');

            // Fetches the data from the database for all the employees
            let response = await fetch('api/employeereport');

            if (!response.ok) // check for response status
                throw new Error(`Status - ${response.status}, Text - ${response.statusText}`);

            // All employee data is then returned in JSON format and if the report was successfully generated then open the PDF 
            // in the new window
            let data = await response.json(); // this returns a promise, so we await it
            (data === 'Report Generated') ? window.open('/Pdfs/Employees.pdf') : $('#lblstatus').text('problem generating report');

            // Let user know the report has been successfully generated
            if (data === 'Report Generated')
                $('#lblstatus').text('Report Generated');
        }
        catch (error) {
            $('#lblstatus').text(error.message);
        } // try catch
    }); // call button click
}); // jQuery