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

function openMetaModal(id, code, year, term) {
    $('#meta_Id').val(id);
    $('#meta_DelId').val(id);
    $('#meta_Code').val(code);
    $('#meta_Year').val(year);
    $('#meta_Term').val(term);

    new bootstrap.Modal(document.getElementById('metaModal')).show();
}

function openSectionsModal(id) {
    // 1. Set Hidden ID
    $('#sec_Id').val(id);

    // reset and close modal
    $('#sectionsModal input[type="text"]').val('');
    $('#sectionsModal input[type="time"]').val('');
    $('#sectionsModal select').val(1);
    $('#sectionsModal .accordion-collapse').removeClass('show');
    $('#sectionsModal .accordion-button').addClass('collapsed').attr('aria-expanded', 'false');

    $('#es_0').addClass('show');
    $('#sectionsModal button[data-bs-target="#es_0"]').removeClass('collapsed').attr('aria-expanded', 'true');

    // show modal
    new bootstrap.Modal(document.getElementById('sectionsModal')).show();

    // window.availableCourses should have been saved into global
    const course = window.availableCourses.find(c => c.Id === id);

    if (!course || !course.Sections) return;

    const maxSections = 5;
    const count = Math.min(course.Sections.length, maxSections);

    for (let i = 0; i < count; i++) {
        const sec = course.Sections[i];

        const secCode = sec.SectionCode || sec.sectionCode;
        $(`input[name="Sections[${i}].SectionCode"]`).val(secCode);

        // expand if data exists
        if (i > 0) {
            $(`#es_${i}`).addClass('show');
            $(`#sectionsModal button[data-bs-target="#es_${i}"]`)
                .removeClass('collapsed')
                .attr('aria-expanded', 'true');
        }

        // fill time slots with global data
        const slots = sec.TimeSlots || sec.timeSlots || [];
        const maxSlots = 2;
        const slotCount = Math.min(slots.length, maxSlots);

        for (let j = 0; j < slotCount; j++) {
            const slot = slots[j];
            const day = slot.Day || slot.day;
            const start = slot.StartTime || slot.startTime;
            const end = slot.EndTime || slot.endTime;

            $(`select[name="Sections[${i}].TimeSlots[${j}].Day"]`).val(day);
            $(`input[name="Sections[${i}].TimeSlots[${j}].StartTime"]`).val(formatTimeForInput(start));
            $(`input[name="Sections[${i}].TimeSlots[${j}].EndTime"]`).val(formatTimeForInput(end));
        }
    }
}

function formatTimeForInput(timeStr) {
    if (!timeStr) return "";
    // API returns "08:00:00", take first 5 chars
    if (timeStr.length >= 5) return timeStr.substring(0, 5);
    return timeStr;
}
function openEditEvalModal(id) {
    var myModal = new bootstrap.Modal(document.getElementById('editEvalModal'));
    myModal.show();

    $.get('/Evaluations/GetEvaluation?id=' + id, function (data) {
        $('#edit_Id').val(data.id);
        $('#edit_Title').val(data.title);
        $('#edit_Type').val(data.type);
        $('#edit_Notes').val(data.notes);

        if (data.dueDate) {
            let dt = new Date(data.dueDate);
            dt.setMinutes(dt.getMinutes() - dt.getTimezoneOffset());
            $('#edit_DueDate').val(dt.toISOString().slice(0, 16));
        }
    });
}

function confirmEvalDelete(id) {
    if (confirm("Are you sure you want to delete this item?")) {
        $('#delete_Id').val(id);
        $('#deleteEvalForm').submit();
    }
}