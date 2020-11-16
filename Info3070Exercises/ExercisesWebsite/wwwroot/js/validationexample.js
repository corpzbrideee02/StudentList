$(function () {

    document.addEventListener("keyup", e => {

        $("#modalstatus").removeClass();    //remove any existing css on div
        if ($("#StudentModalForm").valid()) {
            $("#modalstatus").attr("class", "badge badge-success");//green
            $("#modalstatus").text("data entered is valid");
        }
        else {
            $("#modalstatus").attr("class", "badge badge-danger");//red
            $("#modalstatus").text("fix errors");
        }
    });

    $("#StudentModalForm").validate({
        rules:
        {
            TextBoxTitle: { maxlength: 4, required: true, validTitle: true },
            TextBoxFirstname: { maxlength: 25, required: true },
            TextBoxLastname: { maxlength: 25, required: true },
            TextBoxEmail: { maxlength: 40, required: true, email: true },
            TextBoxPhone: { maxlength: 15, required: true }
        },
        errorElement: "div",
        messages:
        {
            TextBoxTitle: {
                required:"required 1-4 chars.", maxlength: "required 1-4 chars.", validTitle: "Mr. Ms. Mrs. or Dr."
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
                required: "required 1-40 chars.", maxlength: "required 1-40 chars.", email: "need valid email format"
            }

         }

    });//studentModalForm.validate

    $.validator.addMethod("validTitle", (value) => {
        return (value == "Mr." || value === "Ms." || value == "Mrs." || value === "Dr.");
    },"");//validator.addMEthod

    //getButton 
    $("#getbutton").mouseup(async (e) => { //click event handler makes asynchronous fetch to server
        try {
            $("#TextBoxLastname").val("");
            $("#TextBoxEmail").val("");
            $("#TextBoxTitle").val("");
            $("#TextBoxPhone").val("");
            let validator = $("#StudentModalForm").validate();
            validator.resetForm();
            $("#modalstatus").attr("class", "");
            let lastname = $("#TextBoxFindLastname").val();
            $("#theModal").modal("toggle"); //pop the modal
            $("#modalstatus").text("please wait...");
            let response = await fetch(`api/student/${lastname}`);
            if (!response.ok)   //or check for response.status
                throw new Error(`Status - ${response.status}, Text - ${response.statusText}`);
            let data = await response.json();//this returns a promise, so we wait
            if (data.lastname !== "not found") {
                $("#TextBoxEmail").val(data.email);
                $("#TextBoxTitle").val(data.title);
                $("#TextBoxFirstname").val(data.firstname);
                $("#TextBoxLastname").val(data.lastname);
                $("#TextBoxPhone").val(data.phoneno);
                $("#modalstatus").text("student found");
                sessionStorage.setItem("Id", data.Id);
                sessionStorage.setItem("DivisionId", data.DivisionId);
                sessionStorage.setItem("Timer", data.Timer);
            }
            else {
                $("#TextBoxFirstname").val("not found");
                $("#TextBoxLastname").val("");
                $("#TextBoxEmail").val("");
                $("#TextBoxTitle").val("");
                $("#TextBoxPhone").val("");
                $("#modalstatus").text("no such student");
                }
            }
        catch (error) { //catastrophic
            $("#status").text(error.message);
        }//try catch

    });//click event
});