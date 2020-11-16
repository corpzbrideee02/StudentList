$(function () { // employeelist.js

	const getAll = async (msg) => {
		try {
			$("#employeeList").text("Finding Employee Information...");
			let response = await fetch(`api/employee`);
			if (response.ok) {
				let payload = await response.json(); // this returns a promise, so we await it
				buildEmployeeList(payload);
				msg === "" ? // are we appending to an existing message
					$("#status").text("Employee Loaded") : $("#status").text(`${msg} - Employee Loaded`);
			} else if (response.status !== 404) { // probably some other client side error
				let problemJson = await response.json();
				errorRtn(problemJson, response.status);
			} else { // else 404 not found
				$("#status").text("no such path on server");
			} // else
		} catch (error) {
			$("#status").text(error.message);
		}
	}; // getAll

	const clearModalFields = () => {
		$("#TextBoxTitle").val("");
		$("#TextBoxFirstname").val("");
		$("#TextBoxLastname").val("");
		$("#TextBoxPhone").val("");
		$("#TextBoxEmail").val("");
		sessionStorage.removeItem("Id");
		//sessionStorage.removeItem("DivisionId");
		sessionStorage.removeItem("Timer");
	}; // clearModalFields

	$("#employeeList").click((e) => {
		clearModalFields();
		if (!e) e = window.event;
		let id = e.target.parentNode.id;
		if (id === "employeeList" || id === "") {
			id = e.target.id;
		} // clicked on row somewhere else
		if (id !== "status" && id !== "heading") {
			let data = JSON.parse(sessionStorage.getItem("allemployees"));
			data.map(employee => {
				if (employee.id === parseInt(id)) {
					$("#TextBoxTitle").val(employee.title);
					$("#TextBoxFirstname").val(employee.firstname);
					$("#TextBoxLastname").val(employee.lastname);
					$("#TextBoxPhone").val(employee.phoneno);
					$("#TextBoxEmail").val(employee.email);
					sessionStorage.setItem("id", employee.id);
					sessionStorage.setItem("departmentId", employee.departmentId);
					//sessionStorage.setItem("divisionId", employee.divisionId;
					sessionStorage.setItem("timer", employee.timer);
					$("#modalstatus").text("update data");
					$("#theModal").modal("toggle");
				} // if
			}); // data.map
		}//if
		else {
			return false; // ignore if they clicked on heading or status
		}
	});// employeeListClick

	$("#updatebutton").click(async (e) => { // update button click event handler
		try {
			// set up a new client side instance of employee
			emp = new Object();
			// pouplate the properties
			emp.title = $("#TextBoxTitle").val();
			emp.firstname = $("#TextBoxFirstname").val();
			emp.lastname = $("#TextBoxLastname").val();
			emp.phoneno = $("#TextBoxPhone").val();
			emp.email = $("#TextBoxEmail").val();
			// we stored these 3 earlier
			emp.id = parseInt(sessionStorage.getItem("id"));
			emp.departmentId = parseInt(sessionStorage.getItem("departmentId"));
			emp.timer = sessionStorage.getItem("timer");
			//emp.picture64 = null;

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
			} else if (response.status !== 404) { //probably some other client side error
				let problemJson = await response.json();
				errorRtn(problemJson, response.status);
			} else { // else 404 not found
				$("#status").text("no such path on server");
			} // else
		} catch (error) {
			$("#status").text(error.message);
		} // try/catch
		$("#theModal").modal("toggle");
	}); // update button click


	const buildEmployeeList = (data) => {
		$("#employeeList").empty();
		div = $(`<div class="list-group-item text-white row d-flex" id="status" style="background-color:#21768D;">Employee Info</div>
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
	}; // buildEmployeeList
	getAll(""); // first grab the data from the server
}); // jQuery ready method



// server was reached but server had a problem with the call
const errorRtn = (problemJson, status) => {
	if (status > 499) {
		$("#status").text("Problem server side, see debug console");
	} else {
		let keys = Object.keys(problemJson.errors)
		problem = {
			status: status,
			status: problemJson.errors[keys[0]][0], //first error
		};
		$("#status").text("Problem client side, see browser console");
		console.log(problem);
	} // else
}




