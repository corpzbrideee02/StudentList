$(function () { //main jQuery routine- executes every on page load


    //this is declaring a string variable, notice the use of the tick
    //instead of quotes to allow us using multiple lines, here we're 
    //defining 3 students in JSON format

    const stringData =
        `[{ "id": 123, "firstname": "Teachers", "lastname": "Pet"},
    { "id": 234, "firstname": "Brown", "lastname": "Nose" },
    { "id": 345, "firstname": "Always", "lastname": "Late" }]`;

    sessionStorage.getItem("studentData") === null ? sessionStorage.setItem("studentData", stringData) : null;
    let data = JSON.parse(sessionStorage.getItem("studentData"));

    $("#loadbutton").click(e => {
        let html = "";
        data.map(student => {
            html += `<div id="${student.id}"
                    class="list-group-item">  ${student.firstname}, ${student.lastname}</div>`;

        });

        //dump the dynamically genrated html into an element with an
        //id attribute of studentList (in this case a <div></div>)
        $("#studentList").html(html);
        $("#loadbutton").hide();
        $("#addbutton").show();
        $("#removebutton").show();

    });//loadbutton.click()

    $("#studentList").click(e => {
        const student = data.find(s => s.id === parseInt(e.target.id));
        $("#results").text(`you selected ${student.firstname}, ${student.lastname}`);
        //studnet list div click
    });

    //add button event handler
    $("#addbutton").click(e => {

        if (data.length > 0) {
            //find the last student 
            const student = data[data.length - 1];
            data.push({ "id": student.id + 101, "firstname": "new", "lastname": "student" });
            $("#results").text(`adding student ${student.id + 101}`);

        }
        else {

            data.push({ "id": 101, "firstname": "new", "lastname": "student" });
        }

      
        //convert the object array back to a string and put it in session storage
        sessionStorage.setItem("studentData", JSON.stringify(data));

        let html = "";
        data.map(student => {

            html += `<div id="${student.id}"
                    class="list-group-item">  ${student.firstname}, ${student.lastname}</div>`;
        });

        $("#studentList").html(html);

    }); //add button click


    //remove button event handler
    $("#removebutton").click(e => {

        if (data.length > 0) {
            //find the last student 
            const student = data[data.length - 1];
            data.splice(-1, 1);//remove last entry in array

            $("#results").text(`remove student ${student.id}`);

            //put the updated data back to session storage
            sessionStorage.setItem("studentData", JSON.stringify(data));

            let html = "";
            data.map(student => {

                html += `<div id="${student.id}"
                    class="list-group-item">  ${student.firstname}, ${student.lastname}</div>`;
            });
            $("#studentList").html(html);
        }
        else {

            $("#results").text(`no student to remove`);
        }

    }); //remove button click

}); //jQuery routine