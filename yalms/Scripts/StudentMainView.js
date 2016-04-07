//

function enableUploads() {
    var hasAssignment = $("#assignmentId").val() != -1 &&
        $("#assignmentFile").val().length > 4;
    $('#assignmentUploadBtn').prop('disabled', !hasAssignment);
    return true;
};


$(document).ready(function () {
    $("#AssignmentsTree").jstree({
        "plugins": ["themes", "html_data"]
    })
    .on('select_node.jstree', function (e, node) {
        if (node.node.children.length == 0) {
            $("#assignmentLabel").html(node.node.text);
            $("#assignmentId").val(node.node.id);
            $("#assignmentNr").val(node.node.id);
            $('#assignmentDownloadBtn').prop('disabled', false);
            $("#uploadMsg").empty();
            enableUploads();
        }
    })
    $("#assignmentFile").on('change', function () { enableUploads() })

    $("#submissionsTree").jstree({
        "plugins": ["themes", "html_data"]
    })
    .on('select_node.jstree', function (e, node) {
        var href = node.node.a_attr.href
        document.location.href = href;
    })
});

