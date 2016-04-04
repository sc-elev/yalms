//


$(document).ready(function () {
    $("#studentsTree").jstree({
        "plugins": ["themes", "html_data", "checkbox"]
    })
});

function getTreeStudents() {
    var ids = new Array();
    var checked = $("#studentsTree").jstree('get_checked')
    
    alert(checked.join());

    $("#SelectedUsers").val = checked.join();
    return true;
}