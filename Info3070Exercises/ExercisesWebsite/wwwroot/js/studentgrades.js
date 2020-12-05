
$(function () { // studentgrades.js

	const getAll = async (msg) => {
		try {
			$("#studentList").text("Finding Student Information...");
			let response = await fetch(`api/student`);
			if (response.ok) {
				let payload = await response.json(); // this return a promise, so we await it
				buildStudentList(payload);
				msg === "" ? // are we appending to an existing message
					$("#status").text("Students Loaded") : $("#status").text(`${msg} - Student Loaded`);
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

		$("#modaltitle").html("<h4>Gade Entry/Update</h4>");
		clearModalFields();
		data.map(student => {
			if (student.id === parseInt(id)) {
				$("#Firstname").text(student.firstname);
				$("#Lastname").text(student.lastname);
				sessionStorage.setItem("id", student.id);
				sessionStorage.setItem("timer", student.timer);
				$("#modalstatus").text("");
				loadCourseDDL();
				thisStudentGradesDDL();

				$("#theModal").modal("toggle");
			} // if
		}); // data.map
		
	}; // setupForUpdate


	const clearModalFields = () => {
		$("#Firstname").text("");
		$("#Lastname").text("");
		sessionStorage.removeItem("id");
		sessionStorage.removeItem("courseId");
		sessionStorage.removeItem("timer");
		$("#MarkRow").hide();
		$("#CommentsRow").hide();
		$("#actionbutton").hide();
		$("#TextBoxMark").val(0);
		$("#TextBoxComments").val("");
	}; // clearModalFields

	
	const loadCourseDDL = async() => {
		let response = await fetch(`api/course/${sessionStorage.getItem('id')}`);
		if (response.ok) {
			let divs = await response.json(); // this return a promise, so we await it
			sessionStorage.setItem("allcourses", JSON.stringify(divs));
		} else if (response.status !== 404) { // probably some other client side error
			let problemJson = await response.json();
			errorRtn(problemJson, response.status);
		} else { // else 404 not found
			$("#status").text("no such path on server");
		} // else

		html = '';
		$('#ddlCourses').empty();
		let allcourses = JSON.parse(sessionStorage.getItem('allcourses'));
		allcourses.map(cs => html += `<option value="${cs.id}">${cs.name}</option>`);
		$('#ddlCourses').append(html);
		$("#ddlCourses").prop("selectedIndex", -1);



	};//loadDivisionDDL

	const thisStudentGradesDDL = async() => {
		let response = await fetch(`api/grade/${sessionStorage.getItem('id')}`);
		if (response.ok) {
			let divs = await response.json(); // this return a promise, so we await it
			sessionStorage.setItem("studentgrades", JSON.stringify(divs));
			//loadCourseDDL(divs);
		} else if (response.status !== 404) { // probably some other client side error
			let problemJson = await response.json();
			errorRtn(problemJson, response.status);
		} else { // else 404 not found
			$("#status").text("no such path on server");
		} // else

	};//thisStudentGrades

	$("#actionbutton").click(async () => {
		const thisStudentGrades = JSON.parse(sessionStorage.getItem("studentgrades"));
		let grade = thisStudentGrades.find(g => g.courseId === parseInt($('#ddlCourses').val()));
		try {
			// set up a new client side instance of student
			stu = new Object();
			// populate the properties
			stu.courseId = parseInt($("#ddlCourses").val());
			stu.mark = parseInt( $("#TextBoxMark").val());
			stu.comments = $("#TextBoxComments").val();
			stu.studentId = parseInt(sessionStorage.getItem("id"));
			stu.id = parseInt(JSON.stringify(grade.id));
			stu.timer = grade.timer;
		
			// send the updated back to the server asynchronously using PUT
			let response = await fetch("api/grade", {
				method: "PUT",
				headers: { "Content-Type": "application/json; charset=utf-8" },
				body: JSON.stringify(stu)
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
	});

	$('[data-toggle=confirmation]').confirmation({ rootSelector: '[data-toggle=confirmation]' });

	$("#ddlCourses").change((e) => {
		$("#MarkRow").show();
		$("#CommentsRow").show();
		$("#actionbutton").show();
		$("#TextBoxComments").val("");
		sessionStorage.setItem("courseId", parseInt($('#ddlCourses').val()));
		studentgrades = JSON.parse(sessionStorage.getItem("studentgrades"));
		let grade = studentgrades.find(g => g.courseId === parseInt($('#ddlCourses').val()));
		$("#TextBoxMark").val(JSON.stringify(grade.mark));
		if (JSON.stringify(grade.comments) == "null") {
			$("#TextBoxComments").val();
		}
		else {
			$("#TextBoxComments").val(grade.comments);
		}

	});


	$("#studentList").click((e) => {
		if (!e) e = window.event;
		let id = e.target.parentNode.id;
		if (id === "studentList" || id === "") {
			id = e.target.id;
		} // clicked on row somewehere else
		if (id !== "status" && id !== "heading") {
			let data = JSON.parse(sessionStorage.getItem("allstudents"));
			id === "0" ? setupForAdd() : setupForUpdate(id, data);
		} else {
			return false; //ignore if they clicked on heading or status
		}
	}); // studentList click
	const buildStudentList = (data) => {
		$("#studentList").empty();
		div = $(`<div class="list-group-item text-white bg-secondary row d-flex" id="status">Student Info</div>
			<div class= "list-group-item row d-flex text-center" id="heading">
			<div class="col-4 h4">Title</div>
			<div class="col-4 h4">First</div>
			<div class="col-4 h4">Last</div>
		</div>`);
		div.appendTo($("#studentList"));
		sessionStorage.setItem("allstudents", JSON.stringify(data));
		
		data.map(stu => {
			btn = $(`<button class="list-group-item row d-flex" id="${stu.id}">`);
			btn.html(`<div class="col-4" id="studenttitle${stu.id}">${stu.title}</div>
					<div class="col-4" id="studentfname${stu.id}">${stu.firstname}</div>
					<div class="col-4" id="studentlastnam${stu.id}">${stu.lastname}</div>`

			);
			btn.appendTo($("#studentList"));
		}); // map
	}; // buildStudentList

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






