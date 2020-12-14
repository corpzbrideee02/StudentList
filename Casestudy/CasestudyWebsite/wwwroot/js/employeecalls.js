
$(function () { // employeeaddupdate.js

	const getAll = async (msg) => {
		try {
			$("#callList").text("Finding Employee Information...");
			let response = await fetch(`api/call`);
			if (response.ok) {
				let payload = await response.json(); // this return a promise, so we await it
				buildCallList(payload);
				msg === "" ? // are we appending to an existing message
					$("#status").text("Call Loaded") : $("#status").text(`${msg} - Call Loaded`);
			} else if (response.status !== 404) { // probably some other client side error
				let problemJson = await response.json();
				errorRtn(problemJson, response.status);
			} else { // else 404 not found
				$("#status").text("no such path on server");
			} // else

			//get problem data
			response = await fetch(`api/problem`);
			if (response.ok) {
				let divs = await response.json(); // this return a promise, so we await it
				sessionStorage.setItem("allproblems", JSON.stringify(divs));
			} else if (response.status !== 404) { // probably some other client side error
				let problemJson = await response.json();
				errorRtn(problemJson, response.status);
			} else { // else 404 not found
				$("#status").text("no such path on server");
			} // else


			//get employee data
			response = await fetch(`api/employee`);
			if (response.ok) {
				let divs = await response.json(); // this return a promise, so we await it
				sessionStorage.setItem("allemployees", JSON.stringify(divs));
			} else if (response.status !== 404) { // probably some other client side error
				let problemJson = await response.json();
				errorRtn(problemJson, response.status);
			} else { // else 404 not found
				$("#status").text("no such path on server");
			} // else

			//get technician data
			response = await fetch(`api/technician`);
			if (response.ok) {
				let divs = await response.json(); // this return a promise, so we await it
				sessionStorage.setItem("alltechnician", JSON.stringify(divs));
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
		$("#actionbutton").val("Update");
		$("#modaltitle").html("<h4>Update Call</h4>");

		clearModalFields();
		data.map(cl => {
			if (cl.id === parseInt(id)) {

				$("#selEmployee").val(cl.employeeId);
				$("#selProblem").val(cl.problemId);
				$("#selTechnician").val(cl.techId);
				$("#DateOpened").text(formatDate(cl.dateOpened).replace("T", " "));
				$("#TextAreaNotes").val(cl.notes);
				sessionStorage.setItem("id", cl.id);
				sessionStorage.setItem("problemId", cl.problemId);
				sessionStorage.setItem("employeeId", cl.employeeId);
				sessionStorage.setItem("techId", cl.techId);
				sessionStorage.setItem("dateOpened", formatDate(cl.dateOpened).replace("T", " "));
				sessionStorage.setItem("timer", cl.timer);
				$("#modalstatus").text("update data");
				loadProblem(cl.problemId.toString());
				loadEmployee(cl.employeeId.toString());
				loadTechnician(cl.techId.toString());


				//if call is closed
				if (!cl.openStatus) {
					$("#actionbutton").hide();
					$("#DateClosedRow").show();
					$("#CloseCallRow").show();
					$("#DateClosed").text(formatDate(cl.dateClosed).replace("T", " "));
					$("#selEmployee").attr('disabled', true);
					$("#selProblem").attr('disabled', true);
					$("#selTechnician").attr('disabled', true);
					$("#TextAreaNotes").attr('readonly', true);
					$("#myCheck").prop('checked', true);
				}
				else {
					$("#DateClosedRow").show();
					$("#CloseCallRow").show();
					$("#TextAreaNotes").attr('readonly', false);
					$("#selEmployee").attr('disabled', false);
					$("#selProblem").attr('disabled', false);
					$("#selTechnician").attr('disabled', false);
					$("#TextAreaNotes").attr('readonly', false);
					$("#myCheck").prop('checked', false);
                }

				$("#theModal").modal("toggle");
			} // if
		}); // data.map

		$("#deletebutton").show();
	}; // setupForUpdate

	const setupForAdd = () => {
		$("#actionbutton").val("Add");
		$("#modaltitle").html("<h4>Add Call</h4>");
		$("#theModal").modal("toggle");
		$("#modalstatus").text("Add New Call");
		clearModalFields();
		$("#DateOpened").text(formatDate().replace("T", " "));
		sessionStorage.setItem("dateOpened", formatDate());
		$("#deletebutton").hide();
	}; // setupForAdd

	const clearModalFields = () => {
		loadProblem(-1);
		loadEmployee(-1);
		loadTechnician(-1);

		$("#DateClosedRow").hide();
		$("#CloseCallRow").hide();
		
		

		$("#DateOpened").text("");
		$("#TextAreaNotes").val("");


		sessionStorage.removeItem("id");
		sessionStorage.removeItem("problemId");
		sessionStorage.removeItem("employeeId");
		sessionStorage.removeItem("techId");
		sessionStorage.removeItem("dateOpened");
		sessionStorage.removeItem("timer");


		/*$("#selEmployee").attr('disabled', false);
		$("#selProblem").attr('disabled', false);
		$("#selTechnician").attr('disabled', false);
		$("#TextAreaNotes").attr('readonly', false);*/
		//$("#myCheck").prop('checked', false);

	}; // clearModalFields

	const add = async () => {
		try {
			cl = new Object();
			
			cl.id = -1;
			cl.timer = null;
			cl.employeeId = parseInt($("#selEmployee").val());
			cl.problemId = parseInt($("#selProblem").val());
			cl.techId = parseInt($("#selTechnician").val());
			cl.dateOpened = sessionStorage.getItem("dateOpened");
			cl.openStatus = true;
			cl.notes = $("#TextAreaNotes").val();


			//send the call info to the server asynchronously using POST
			let response = await fetch("api/call", {
				method: "POST",
				headers: {
					"Content-Type": "application/json; charset=utf-8"
				},
				body: JSON.stringify(cl)
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
			
			cl = new Object();

			cl.id = parseInt(sessionStorage.getItem("id"));
			cl.timer = sessionStorage.getItem("timer");
			cl.employeeId = parseInt($("#selEmployee").val());
			cl.problemId = parseInt($("#selProblem").val());
			cl.techId = parseInt($("#selTechnician").val());
			cl.dateOpened = sessionStorage.getItem("dateOpened");

			cl.notes = $("#TextAreaNotes").val();

			if (sessionStorage.getItem("openStatus")==true) {

				cl.openStatus = true;
			}
			else {
				cl.openStatus = false;

				cl.dateClosed = sessionStorage.getItem("dateClosed");
            }

			// send the updated back to the server asynchronously using PUT
			let response = await fetch("api/call", {
				method: "PUT",
				headers: { "Content-Type": "application/json; charset=utf-8" },
				body: JSON.stringify(cl)
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
			let response = await fetch(`api/call/${sessionStorage.getItem('id')}`, {
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

	const loadProblem = (probdiv) => {
		html = '';
		$('#selProblem').empty();
		let allproblems = JSON.parse(sessionStorage.getItem('allproblems'));
		allproblems.map(emp => html += `<option value="${emp.id}">${emp.description}</option>`);
		$('#selProblem').append(html);
		$('#selProblem').val(probdiv);
	};//loadProblem


	const loadEmployee = (empdiv) => {
		html = '';
		$('#selEmployee').empty();
		let allemployees = JSON.parse(sessionStorage.getItem('allemployees'));
		allemployees.map(emp => html += `<option value="${emp.id}">${emp.lastname}</option>`);
		$('#selEmployee').append(html);
		$('#selEmployee').val(empdiv);
	};//loadEmployee


	const loadTechnician = (empdiv) => {
		html = '';
		$('#selTechnician').empty();
		let alltechnician = JSON.parse(sessionStorage.getItem('alltechnician'));
		alltechnician.map(emp => html += `<option value="${emp.id}">${emp.lastname}</option>`);
		$('#selTechnician').append(html);
		$('#selTechnician').val(empdiv);
	};//loadTechnician


	const formatDate = (date) => {
		let d;
		(date === undefined) ? d = new Date() : d = new Date(Date.parse(date));
		let _day = d.getDate();
		if (_day < 10) { _day = "0" + _day; }
		let _month = d.getMonth()+1;
		if (_month < 10) { _month = "0" + _month; }
		let _year = d.getFullYear();
		let _hour = d.getHours();
		if (_hour < 10) { _hour = "0" + _hour; }
		let _min = d.getMinutes();
		if (_min < 10) { _min = "0" + _min; }
		return _year + "-" + _month + "-" + _day + "T" + _hour + ":" + _min;
	}//formatDate 

	$("#actionbutton").click(() => {
		$("#actionbutton").val() === "Update" ? update() : add();
	});

	$('[data-toggle=confirmation]').confirmation({ rootSelector: '[data-toggle=confirmation]' });
	$('#deletebutton').click(() => _delete());// if yes was chosen


	//myCheck.click
	$("#myCheck").click(() => {
		if ($("#myCheck").is(":checked")) {
			$("#DateClosed").text(formatDate().replace("T", " "));
			sessionStorage.setItem("dateClosed", formatDate());
			sessionStorage.setItem("openStatus", false);
		}
		else {
			$("#DateClosed").text("");
			sessionStorage.setItem("dateClosed", "");
			sessionStorage.setItem("openStatus", true);
        }
	});




	//search keyup()
	$("#srch").keyup(() => {
		let alldata = JSON.parse(sessionStorage.getItem("allcalls"));
		let filtereddata = alldata.filter((emp) => emp.employeeName.match(new RegExp($("#srch").val(), 'i')));
		buildCallList(filtereddata, false);
	});	//srch keyup

	$("#callList").click((e) => {
		if (!e) e = window.event;
		let id = e.target.parentNode.id;
		if (id === "callList" || id === "") {
			id = e.target.id;
		} // clicked on row somewehere else
		if (id !== "status" && id !== "heading") {
			let data = JSON.parse(sessionStorage.getItem("allcalls"));
			id === "0" ? setupForAdd() : setupForUpdate(id, data);
		} else {
			return false; //ignore if they clicked on heading or status
		}
	}); // callList click




	const buildCallList = (data, usealldata = true) => {
		btn = $(`<button class="list-group-item row d-flex" id="0"><img class="card-img" src="img/stu.png" alt="Add Call" style="width:5%; margin:0 auto;"></button>`);

		$("#callList").empty();
		btn.appendTo($("#callList"));
		div = $(`<div class="list-group-item text-white row d-flex" id="status" style="background-color:#21768D;">Call Info</div>
			<div class= "list-group-item row d-flex text-center" id="heading">
			<div class="col-4 h4">Date</div>
			<div class="col-4 h4">For</div>
			<div class="col-4 h4">Problem</div>
		</div>`);
		div.appendTo($("#callList"));
		usealldata ? sessionStorage.setItem("allcalls", JSON.stringify(data)) : null;
		data.map(emp => {
			btn = $(`<button class="list-group-item row d-flex" id="${emp.id}">`);
			//formatDate(cl.dateOpened).replace("T", " ")
			//btn.html(`< div class= "col-4" id = "employeetitle${emp.id}" > ${emp.dateOpened }</div >
			btn.html(`<div class="col-4" id="employeetitle${emp.id}">${formatDate(emp.dateOpened).replace("T", " ")}</div>
					<div class="col-4" id="employeefname${emp.id}">${emp.employeeName}</div>
					<div class="col-4" id="employeelastnam${emp.id}">${emp.problemDescription}</div>`

			);
			btn.appendTo($("#callList"));
		}); // map


	}; // buildCallList

	getAll(""); // first grab the data form the server
}); // jQuery ready method

// server was reached but server had a problem with the call
const errorRtn = (problemJson, status) => {
	if (status > 499) {
		$("#status").text("problem server side, see debug console");
	} else {
		let keys = Object.keys(problemJson.errors)
		problem = {
			status: status,
			statusText: problemJson.errors[keys[0]][0], // firs error
		};
		$("#status").text("Problem client side, see browser console");
		console.log(problem);
	} // else
}






