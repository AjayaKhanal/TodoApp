$(document).ready(function () {
	var status = $('.status');
	var statusText;
	var priority = $('.priority').text();
	// console.log(status);

	$('.status').each(function () {
		statusText = $(this).text().trim(); // Trim to remove any extra spaces

		if (statusText === "Pending") {
			$(this).addClass("pendingStatus");
		}
		else if (statusText === "InProgress") {
			$(this).addClass("inprogressStatus");
		}
		else if (statusText === "Completed") {
			$(this).addClass("completedStatus");
		} else {
			console.log("Invalid status");
		}
	});

	setInterval(updateCountdown, 1000);
	updateCountdown(); // initial call
});

function updateCountdown() {
	const elements = document.querySelectorAll('[id^="remaining-"]');
	const now = new Date();
	var status = document.querySelectorAll('.completedStatus');
	// console.log(now);

	elements.forEach(el => {
		const dueDate = new Date(el.dataset.duedate);
		dueDate.setHours(23, 59, 59, 999); // Set to end of the due day
		// console.log(dueDate);
		const statusEl = el.closest('.todo-card')?.querySelector('.status');
		const statusText = statusEl?.textContent.trim();
		// console.log(statusText);
		// If status is Completed, show 0d 0:0:0 and return
		if (statusText === "Completed") {
			el.textContent = '0d 0:0:0';
			el.style.color = '';
			el.style.fontWeight = '';
			return;
		}

		const diff = dueDate - now;

		if (diff > 0) {
			const totalSeconds = Math.floor(diff / 1000);
			const days = Math.floor(totalSeconds / (3600 * 24));
			const hours = Math.floor((totalSeconds % (3600 * 24)) / 3600);
			const minutes = Math.floor((totalSeconds % 3600) / 60);
			const seconds = totalSeconds % 60;

			el.textContent = `${days}d ${hours}:${minutes}:${seconds}`;

			// Highlight if less than 24 hours
			if (totalSeconds < 86400) {
				el.style.color = "red";
				el.style.fontWeight = "bold";
			} else {
				el.style.color = "";
				el.style.fontWeight = "";
			}
		} else {
			el.textContent = "Expired";
			el.style.color = "red";
			el.style.fontWeight = "bold";
			return;
		}


	});
}