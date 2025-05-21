$(document).ready(function () {
	// Function to set a cookie
	function setCookie(name, value, days) {
		var expires = "";
		if (days) {
			var date = new Date();
			date.setTime(date.getTime() + (days * 24 * 60 * 60 * 1000));
			expires = "; expires=" + date.toUTCString();
		}
		document.cookie = name + "=" + (value || "") + expires + ";path=/";
		console.log(document.cookie);
	}

	// Function to get a cookie
	function getCookie(name) {
		var nameCookie = name + "=";
		console.log(nameCookie);
		var ca = document.cookie.split(';');
		for (var i = 0; i < ca.length; i++) {
			var c = ca[i].trim();
			if (c.indexOf(nameCookie) === 0) {
				return c.substring(nameCookie.length);
			}
		}
		return null;
	}

	function applyTheme(theme) {
		$('body').removeClass('light dark');
		$('nav').removeClass('navbar-light bg-white navbar-dark bg-black');
		$('.nav-link').removeClass('text-dark text-light');
		// $('.setting-container').removeClass('dark-setting-container light-setting-container');
		// $('.card').removeClass('dark-card light-card');

		if (theme === 'light') {
			$('body').addClass('light');
			$('nav').addClass('navbar-light bg-white');
			$('.nav-link').addClass('text-dark');
			// $('.setting-container').addClass('light-setting-container');
			// $('.card').addClass('light-card');
		}

		if (theme === 'dark') {
			$('body').addClass('dark');
			$('nav').addClass('navbar-dark bg-black');
			$('.nav-link').addClass('text-light');
			// $('.setting-container').addClass('dark-setting-container');
			// $('.card').addClass('dark-card');
		}

		if (theme === 'system') {
			// 'system' theme: detect system preference
			if (window.matchMedia && window.matchMedia('(prefers-color-scheme: dark)').matches) {
				$('body').addClass('dark');
				$('nav').addClass('navbar-dark bg-black');
				$('.nav-link').addClass('text-light');
			} else {
				$('body').addClass('light');
				$('nav').addClass('navbar-light bg-white');
				$('.nav-link').addClass('text-dark');
			}
		}
	}

	const cookieTheme = getCookie("theme") || "system";

	// Sync dropdown only if it's on the page
	if ($('#theme-select').length) {
		$('#theme-select').val(cookieTheme); // Sync to cookie
		$('#theme-select').on('change', function () {
			const selected = $(this).val();
			setCookie("theme", selected, 30);
			applyTheme(selected); //immediately preview change
		});
	}

	applyTheme(cookieTheme);
});