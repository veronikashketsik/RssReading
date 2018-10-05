jQuery(document).ready(function ($) {
	$('.pagination-page').click(getRssItemsPage);
});

function getRssItemsPage(e) {
	e.preventDefault();
	var pageNum = jQuery(this).text();
	$.ajax({
		url: "/rss/filterRssItems?paging.page=" + pageNum + "&paging.pageSize=10",
		data: $('#form').serialize(),
		type: 'post',
		success: function (data) {
			if (data !== null || data !== "") {
				$('#news').empty();
				$('#news').append(data);
				$(window).scrollTop(0);
			}
		}
	});
}

