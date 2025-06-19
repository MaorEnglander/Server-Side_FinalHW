const idBtn = document.querySelector("#IdSearchBtn");
const updateBtn = document.querySelector("#updateBtn");
const createBtn = document.querySelector("#createBtn");
const deleteBtn = document.querySelector("#deleteBtn");
let saveBtn = document.querySelector("#saveBtn"); // מוגדר מחדש לאחר שיבוט
const eventDate = document.querySelector("#eventDate");
const myEvents = document.querySelector("#myEvents");
const form = document.querySelector("#form");

function formatDateEnglish(isoDateString) {
    const date = new Date(isoDateString);
    return date.toLocaleDateString("en-US", {
        weekday: 'long',
        year: 'numeric',
        month: 'long',
        day: 'numeric'
    });
}

function fetchEvent() {
    const idInput = document.querySelector("#IdSearchInput").value;
    const url = `http://localhost:5048/event/${idInput}`;

    return fetch(url)
        .then(response => {
            if (!response.ok) throw new Error("Failed to fetch event");
            return response.json();
        });
}

function fetchEventsByDate(date) {
    const url = `http://localhost:5048/event/bydate?date=${date}`;
    return fetch(url)
        .then(response => {
            if (!response.ok) throw new Error("Failed to fetch events by date");
            return response.json();
        });
}

function createEventElement(event) {
    const eventElement = document.createElement("div");
    eventElement.className = "eventStyle";

    const name = document.createElement("h3");
    name.innerText = event.name;

    const startDate = document.createElement("p");
    startDate.innerText = "Start date: " + formatDateEnglish(event.startDate);

    const endDate = document.createElement("p");
    endDate.innerText = "End date: " + formatDateEnglish(event.endDate);

    const maxReg = document.createElement("p");
    maxReg.innerText = "Max Registrations: " + event.maxRegistrations;

    const location = document.createElement("p");
    location.innerText = "Location: " + event.location;

    [name, startDate, endDate, maxReg, location].forEach(el => eventElement.appendChild(el));
    myEvents.appendChild(eventElement);
}

function ShowEventOnForm(event) {
    const nameContainer = document.querySelector("#name");
    const startInput = document.querySelector("#startInput");
    const endInput = document.querySelector("#endInput");
    const maxInput = document.querySelector("#maxInput");
    const locationSelect = document.querySelector("#locationSelect");

    if (nameContainer) nameContainer.innerText = event.name || "";

    startInput.value = event.startDate?.split("T")[0] || "";
    endInput.value = event.endDate?.split("T")[0] || "";
    maxInput.value = event.maxRegistrations;

    const locationValue = event.location || "";
    const exists = [...locationSelect.options].some(opt => opt.value === locationValue);
    if (!exists && locationValue !== "") {
        const opt = document.createElement("option");
        opt.value = locationValue;
        opt.innerText = beautifyLocation(locationValue);
        locationSelect.appendChild(opt);
    }
    locationSelect.value = locationValue;
}

function beautifyLocation(str) {
    if (/[֐-׿]/.test(str)) return str;
    return str.replace(/[-_]+/g, " ").replace(/\b\w/g, c => c.toUpperCase());
}

function enableNameEditing() {
    if (document.querySelector("#nameInput")) return;

    const nameDiv = document.querySelector("#name");
    if (!nameDiv) return;

    const input = document.createElement("input");
    input.id = "nameInput";
    input.type = "text";
    input.value = nameDiv.innerText;
    nameDiv.replaceWith(input);
}

function clearFormInputs() {
    document.querySelector("#startInput").value = "";
    document.querySelector("#endInput").value = "";
    document.querySelector("#maxInput").value = "";
    document.querySelector("#locationSelect").selectedIndex = 0;

    const nameInput = document.querySelector("#nameInput");
    if (nameInput) nameInput.value = "";
    else enableNameEditing();
}

function createEvent() {
    const nameInput = document.querySelector("#nameInput");
    const start = document.querySelector("#startInput").value;
    const end = document.querySelector("#endInput").value;
    const max = parseInt(document.querySelector("#maxInput").value);
    const location = document.querySelector("#locationSelect").value;

    const data = {
        name: nameInput.value,
        startDate: new Date(start).toISOString(),
        endDate: new Date(end).toISOString(),
        maxRegistrations: max,
        location: location
    };

    fetch("http://localhost:5048/event", {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(data)
    })
        .then(res => {
            if (!res.ok) throw new Error("Failed to create event");
            return res.json();
        })
        .then(data => {
            const nameDiv = document.createElement("div");
            nameDiv.id = "name";
            nameDiv.innerText = data.name;
            nameInput.replaceWith(nameDiv);
        })
        .catch(err => console.error("Error creating event:", err));
}

function updateEvent(id) {
    const startDate = new Date(document.querySelector("#startInput").value).toISOString();
    const endDate = new Date(document.querySelector("#endInput").value).toISOString();
    const max = parseInt(document.querySelector("#maxInput").value);
    const location = document.querySelector("#locationSelect").value;

    const data = {
        startDate,
        endDate,
        maxRegistrations: max,
        location
    };

    fetch(`http://localhost:5048/event/${id}`, {
        method: 'PUT',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(data)
    })
        .then(res => {
            if (!res.ok) throw new Error("Failed to update event");
            console.log("Event updated successfully");
        })
        .catch(err => console.error("Error updating event:", err));
}

// ========== Event Listeners ==========

idBtn.addEventListener("click", () => {
    form.classList.add("hide");
    myEvents.classList.remove("hide");
    myEvents.innerHTML = "";

    fetchEvent()
        .then(createEventElement)
        .catch(() => {
            myEvents.innerHTML = `<p style="color: red;">Event not found or server error.</p>`;
        });
});

updateBtn.addEventListener("click", () => {
    form.classList.remove("hide");
    myEvents.classList.add("hide");

    fetchEvent().then(event => {
        ShowEventOnForm(event);

        const nameInput = document.querySelector("#nameInput");
        if (nameInput) {
            const div = document.createElement("div");
            div.id = "name";
            div.innerText = event.name || "";
            nameInput.replaceWith(div);
        }

        const newSave = saveBtn.cloneNode(true);
        saveBtn.parentNode.replaceChild(newSave, saveBtn);
        saveBtn = newSave;
        saveBtn.addEventListener("click", () => {
            const id = document.querySelector("#IdSearchInput").value;
            updateEvent(id);
        });
    }).catch(() => {
        myEvents.innerHTML = `<p style="color: red;">Event not found or server error.</p>`;
    });
});

createBtn.addEventListener("click", () => {
    form.classList.remove("hide");
    myEvents.classList.add("hide");
    clearFormInputs();

    const newSave = saveBtn.cloneNode(true);
    saveBtn.parentNode.replaceChild(newSave, saveBtn);
    saveBtn = newSave;
    saveBtn.addEventListener("click", () => {
        createEvent();
    });
});

deleteBtn.addEventListener("click", () => {
    const id = document.querySelector("#IdSearchInput").value;
    fetch(`http://localhost:5048/event/${id}`, {
        method: 'DELETE'
    })
        .then(res => {
            if (!res.ok) throw new Error("Failed to delete event");
            myEvents.classList.remove("hide");
            form.classList.add("hide");
            myEvents.innerHTML = `<p style="color: green;">Event deleted successfully.</p>`;
        })
        .catch(() => {
            myEvents.innerHTML = `<p style="color: red;">Error deleting event.</p>`;
        });
});

eventDate.addEventListener("change", () => {
    const selectedDate = eventDate.value;
    if (!selectedDate) return;

    form.classList.add("hide");
    myEvents.classList.remove("hide");
    myEvents.innerHTML = "";

    fetchEventsByDate(selectedDate).then(events => {
        if (events.length === 0) {
            myEvents.innerHTML = `<p>No events on selected date.</p>`;
        } else {
            events.forEach(createEventElement);
        }
    }).catch(() => {
        myEvents.innerHTML = `<p style='color:red;'>Error loading events.</p>`;
    });
});
