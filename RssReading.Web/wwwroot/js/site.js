jQuery(document).ready(function ($) {
	//$('#button').click(getFilteredRssItems);
	$('#form input, #form select').change(getFilteredRssItems);
});

function getFilteredRssItems(e) {
	e.preventDefault();
	$.ajax({
		url: "/rss/filterRssItems",
		data: $('#form').serialize(),
		type: 'post',
		success: function (data) {
			if (data !== null || data !== "") {
				$('#news').empty();
				$('#news').append(data);
			}
		}
	});
}

