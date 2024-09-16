$("#GroupId").change(function () {
    $("#SubGroupId").empty();
    $.getJSON("/Admin/Product/GetSubGroups/" + $("#GroupId :selected").val(),
        function (data) {

            $.each(data,
                function () {
                    $("#SubGroupId").append('<option value=' + this.value + '>' + this.text + '</option>');

                });

        });


});