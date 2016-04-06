//


$(document).ready(function () {
    $("#studentsTree").jstree({
        "plugins": ["themes", "html_data", "checkbox"],
        "checkbox" : {
            "three_state" : false,
        },
    })
    if (location.hash) {
        $('a[href=' + location.hash + ']').tab('show');
    }
    $(document.body).on("click", "a[data-toggle]", function (event) {
        location.hash = this.getAttribute("href");
    })
    $("#ClassDropDown").change(function () {
        var classId = $("#ClassDropDown").val();
        $("#ClassListResults").load(
            '/Admin/GetClassList?SelectedClass=' + classId);
    });
});

$(window).on('popstate', function () {
    var anchor = location.hash || $("a[data-toggle=tab]").first().attr("href");
    $('a[href=' + anchor + ']').tab('show');
});

function getTreeStudents() {
    var users = $("#studentsTree").jstree('get_checked').join();
    var class_ = $("#ClassDropDown").val();
    var url = '/Admin/AddClassStudents?SelectedUsers=' + users + '&SelectedClass=' + class_
    location.href = url
    return false
}