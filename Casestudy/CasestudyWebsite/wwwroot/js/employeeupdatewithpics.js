$(function () {


	document.addEventListener("keyup", e => {


		$("#modalstatus").removeClass();    //remove any existing css on div
		if ($("#EmployeeModalForm").valid()) {

			$("#actionbutton").prop('disabled', false);
			$("#modalstatus").attr("class", "badge badge-success");//green
			$("#modalstatus").text("data entered is valid");
		}
		else {

			$("#actionbutton").prop('disabled', true);
			$("#modalstatus").attr("class", "badge badge-danger");//red
			$("#modalstatus").text("fix errors");
		}
	});


	$("#EmployeeModalForm").validate({
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
				required: "required 1-40 chars.", maxlength: "required 1-40 chars.", email: "need valid email format"
			}

		}

	});//EmployeeModalForm.validate


	$.validator.addMethod("validTitle", (value) => {
		return (value == "Mr." || value === "Ms." || value == "Mrs." || value === "Dr.");
	}, "");//validator.addMEthod

	//do we have picture?
	$("input:file").change(() => {
		const reader = new FileReader();
		const file = $("#uploader")[0].files[0];

		file ? reader.readAsBinaryString(file) : null;

		reader.onload = (readerEvt) => {
			//get binary data then convert to encoded string
			const binaryString = reader.result;
			const encodedString = btoa(binaryString);
			sessionStorage.setItem("picture", encodedString);


		};

	});//input:file change

	const getAll = async (msg) => {
		try {
			$("#employeeList").text("Finding Employee Information...");
			let response = await fetch(`api/employee`);
			if (response.ok) {
				let payload = await response.json(); // this return a promise, so we await it
				buildEmployeeList(payload);
				msg === "" ? // are we appending to an existing message
					$("#status").text("Employees Loaded") : $("#status").text(`${msg} - Employee Loaded`);
			} else if (response.status !== 404) { // probably some other client side error
				let problemJson = await response.json();
				errorRtn(problemJson, response.status);
			} else { // else 404 not found
				$("#status").text("no such path on server");
			} // else

			//get department data
			response = await fetch(`api/department`);
			if (response.ok) {
				let divs = await response.json(); // this return a promise, so we await it
				sessionStorage.setItem("alldepartments", JSON.stringify(divs));
			} else if (response.status !== 404) { // probably some other client side error
				let problemJson = await response.json();
				errorRtn(problemJson, response.status);
			} else { // else 404 not found
				$("#status").text("no such path on server");
			} // else


		} catch (error) {
			$("#status").text(error.message);
		}
	}; //getAll

	const setupForUpdate = (id, data) => {
		$("#actionbutton").val("update");
		$("#modaltitle").html("<h4>Update Employee</h4>");

		clearModalFields();
		data.map(employee => {
			if (employee.id === parseInt(id)) {
				$("#TextBoxTitle").val(employee.title);
				$("#TextBoxFirstname").val(employee.firstname);
				$("#TextBoxLastname").val(employee.lastname);
				$("#TextBoxPhone").val(employee.phoneno);
				$("#TextBoxEmail").val(employee.email);

				$("#ImageHolder").html(`<img height="120" width="110" src="data:img/png;base64,${employee.staffPicture64}"/> `);
				sessionStorage.setItem("id", employee.id);
				sessionStorage.setItem("departmentId", employee.departmentId);
				sessionStorage.setItem("timer", employee.timer);
				sessionStorage.setItem("picture", employee.staffPicture64);

				$("#modalstatus").text("update data");
				loadDepartmentDDL(employee.departmentId.toString());
				$("#theModal").modal("toggle");
			} // if
		}); // data.map

		$("#deletebutton").show();
	}; // setupForUpdate

	const setupForAdd = () => {
		$("#actionbutton").val("add");
		$("#modaltitle").html("<h4>Add Employee</h4>");
		$("#theModal").modal("toggle");
		$("#modalstatus").text("Add New Employee");
		clearModalFields();
		$("#deletebutton").hide();
	}; // setupForAdd

	const clearModalFields = () => {
		loadDepartmentDDL(-1);
		$("#TextBoxTitle").val("");
		$("#TextBoxFirstname").val("");
		$("#TextBoxLastname").val("");
		$("#TextBoxPhone").val("");
		$("#TextBoxEmail").val("");
		sessionStorage.removeItem("id");
		sessionStorage.removeItem("departmentId");
		sessionStorage.removeItem("timer");
		sessionStorage.removeItem("picture");
		$("#EmployeeModalForm").validate().resetForm();
	}; // clearModalFields

	const add = async () => {
		try {
			emp = new Object();
			emp.title = $("#TextBoxTitle").val();
			emp.firstname = $("#TextBoxFirstname").val();
			emp.lastname = $("#TextBoxLastname").val();
			emp.phoneno = $("#TextBoxPhone").val();
			emp.email = $("#TextBoxEmail").val();
			emp.departmentId = parseInt($("#ddlDepartments").val());

			emp.id = -1;
			emp.timer = null;
			emp.picture64 = null;
			//send the employee info to the server asynchronously using POST
			let response = await fetch("api/employee", {
				method: "POST",
				headers: {
					"Content-Type": "application/json; charset=utf-8"
				},
				body: JSON.stringify(emp)
			});
			if (response.ok) // or check for response.status
			{
				let data = await response.json();
				getAll(data.msg);
			} else if (response.status !== 404) { // probably some other client side error
				let problemJson = await response.json();
				errorRtn(problemJson, response.status);
			} else { // else 404 not found
				$("#status").text("no such path on server");
			} // else
		} catch (error) {
			$("#status").text(error.message);
		} // try/catch
		$("#theModal").modal("toggle");


	}; // add

	const update = async () => {
		try {


			// set up a new client side instance of employee
			emp = new Object();
			// pouplate the properties
			emp.title = $("#TextBoxTitle").val();
			emp.firstname = $("#TextBoxFirstname").val();
			emp.lastname = $("#TextBoxLastname").val();
			emp.phoneno = $("#TextBoxPhone").val();
			emp.email = $("#TextBoxEmail").val();
			emp.departmentId = parseInt($("#ddlDepartments").val());

			emp.id = parseInt(sessionStorage.getItem("id"));
			emp.departmentId = parseInt(sessionStorage.getItem("departmentId"));
			emp.timer = sessionStorage.getItem("timer");

			sessionStorage.getItem("picture")
				? emp.staffPicture64 = sessionStorage.getItem("picture")
				: emp.staffPicture64 = null;

			// send the updated back to the server asynchronously using PUT
			let response = await fetch("api/employee", {
				method: "PUT",
				headers: { "Content-Type": "application/json; charset=utf-8" },
				body: JSON.stringify(emp)
			});
			if (response.ok) // or check for response.status
			{
				let data = await response.json();
				getAll(data.msg);
			} else if (response.status !== 404) { // probably some other client side error
				let problemJson = await response.json();
				errorRtn(problemJson, response.status);
			} else { // else 404 not found
				$("#status").text("no such path on server");
			} // else
		} catch (error) {
			$("#status").text(error.message);
		} // try/catch
		$("#theModal").modal("toggle");
	};

	const _delete = async () => {
		try {
			let response = await fetch(`api/employee/${sessionStorage.getItem('id')}`, {
				method: 'DELETE',
				headers: { 'Content-Type': 'application/json; charset=utf-8' }
			});
			if (response.ok) // or check for response.status
			{
				let data = await response.json();
				getAll(data.msg);
			} else if (response.status !== 404) { // probably some other client side error
				let problemJson = await response.json();
				errorRtn(problemJson, response.status);
			} else { // else 404 not found
				$("#status").text("no such path on server");
			} // else
		} catch (error) {
			$("#status").text(error.message);
		}

		// try/catch
		$("#theModal").modal("toggle");
	};//delete

	const loadDepartmentDDL = (empdiv) => {
		html = '';
		$('#ddlDepartments').empty();
		let alldepartments = JSON.parse(sessionStorage.getItem('alldepartments'));
		alldepartments.map(emp => html += `<option value="${emp.id}">${emp.name}</option>`);
		$('#ddlDepartments').append(html);
		$('#ddlDepartments').val(empdiv);
	};//loadDivisionDDL


	$("#actionbutton").click(() => {
		$("#actionbutton").val() === "update" ? update() : add();
	});

	$('[data-toggle=confirmation]').confirmation({ rootSelector: '[data-toggle=confirmation]' });
	$('#deletebutton').click(() => _delete());// if yes was chosen

	$("#employeeList").click((e) => {
		if (!e) e = window.event;
		let id = e.target.parentNode.id;
		if (id === "employeeList" || id === "") {
			id = e.target.id;
		} // clicked on row somewehere else
		if (id !== "status" && id !== "heading") {
			let data = JSON.parse(sessionStorage.getItem("allemployees"));
			id === "0" ? setupForAdd() : setupForUpdate(id, data);
		} else {
			return false; //ignore if they clicked on heading or status
		}



	}); // employeeList click
	const buildEmployeeList = (data) => {
		btn = $(`<button class="list-group-item row d-flex" id="0"><img class="card-img" src="img/stu.png" alt="Add Employee" style="width:5%; margin:0 auto;"></button>`);

		$("#employeeList").empty();
		btn.appendTo($("#employeeList"));
		div = $(`<div class="list-group-item text-white row d-flex" id="status" style="background-color:#21768D;">employee Info</div>
			<div class= "list-group-item row d-flex text-center" id="heading">
			<div class="col-4 h4">Title</div>
			<div class="col-4 h4">First Name</div>
			<div class="col-4 h4">Last Name</div>
		</div>`);
		div.appendTo($("#employeeList"));
		sessionStorage.setItem("allemployees", JSON.stringify(data));

		data.map(emp => {
			btn = $(`<button class="list-group-item row d-flex" id="${emp.id}">`);
			btn.html(`<div class="col-4" id="employeetitle${emp.id}">${emp.title}</div>
					<div class="col-4" id="employeefname${emp.id}">${emp.firstname}</div>
					<div class="col-4" id="employeelastnam${emp.id}">${emp.lastname}</div>`

			);
			btn.appendTo($("#employeeList"));
		}); // map

	}; // buildemployeeList

	getAll(""); // first grab the data form the server

});