// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
let globalSchedules = [];

// init page from model
function initPlanner(data) {
    globalSchedules = data;

    // setup dynamic schedule display buttons
    $('.schedule-btn').on('click', function () {

        $('.schedule-btn').removeClass('active');
        $(this).addClass('active');


        const id = $(this).data('id');
        const plan = globalSchedules.find(p => p.Id === id);

        if (plan) {
            $('#planTitle').text(plan.Name);
            drawGrid(plan.Classes);
        }
    });

    // auto select the first one if any exist
    if (globalSchedules.length > 0) {
        $('.schedule-btn').first().trigger('click');
    }

    // enable/disable generate button on selected courses
    $('.course-selector').on('change', function () {
        $('#btnGenerate').prop('disabled', $('.course-selector:checked').length === 0);
    });

    $('#btnGenerate').prop('disabled', $('.course-selector:checked').length === 0);
}

function drawGrid(classes) {
    // clear old course plan
    $('.class-block').remove();

    classes.forEach(cls => {
        // DayOfWeek enum: Mon=1, Fri=5
        if (cls.Day < 1 || cls.Day > 5) return;

        const cellId = `#cell-${cls.Day}-${cls.StartHour}`;
        const $cell = $(cellId);

        if ($cell.length) {
            // 50px per hour
            const height = (cls.Duration * 50) - 2;

            const html = `
                <div class="class-block" style="height: ${height}px; top: 1px;">
                    <strong>${cls.CourseCode}</strong><br>
                    <span>${cls.SectionCode}</span><br>
                    <span>${cls.TimeLabel}</span>
                </div>
            `;
            $cell.append(html);
        }
    });
}