;
$(function () {
    // jQuery Selector (DOM 關聯) 需根據不同的 View layout 調整

    $("form.form-search")
        .each(function (i, el) {
            var $form = $(this),
                $submit = $form.find("[type=submit]"),
                $paging = $form.parents(".paging"),
                $table = $paging.find("table:first"),
                $sortable = $table.find("tr:first").find(".sortable"),
                $pager = $paging.find(".pagination");

            $sortable.click(function () {
                var $this = $(this),
                    $icon = $this.find("i.icon"),
                    column = $this.data("column"),
                    order = $this.data("direction");

                switch (order) {
                    case "Ascending":
                        nextSort(column, "Descending");
                        $icon.attr("class", "icon icon-chevron-down");
                        break;
                    case "Descending":
                        nextSort(null, null);
                        $icon.removeClass("icon");
                        break;
                    default:
                        nextSort(column, "Ascending");
                        $this.append(" <i class='icon icon-chevron-up' />");
                        break;
                }

                return false;
            });

            var nextSort = function (column, order) {
                $form.find("[name=Column]").val(column);
                $form.find("[name=Order]").val(order);
                $form.trigger("submit");
            };

            $submit.click(function () {
                $form.find("[name=Page]").val(1);
                $form.trigger("submit");
            });

            $pager.on("click", "a", function (evt) {
                var $page = $form.find("[name=Page]"),
                    page = parseInt(this.href.match(/(\d+)$/)[1]);

                if (isNaN(page))
                    return false;

                $page.val(page);
                $form.trigger("submit");

                return false;
            });
        });

});