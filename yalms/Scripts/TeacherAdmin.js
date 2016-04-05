//


$(document).ready(function () {
    $("#studentsTree").jstree({
        "plugins": ["themes", "html_data", "checkbox"]
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
    var ids = new Array();
    var checked = $("#studentsTree").jstree('get_checked')
    
    alert(checked.join());

    $("#SelectedUsers").val = checked.join();
    return true;
}