$('document').ready(function () {
    function init() {
        HaveNotSeen();
    }

    function HaveNotSeen() {
        $(".have-not-seen").on("click", function () {
            var $this = $(this);
            var $inputRating = $this.parent().find('.input-rating');
            $inputRating.val('0');
            $inputRating.attr('min', 0);
        });
    }

    init();
});