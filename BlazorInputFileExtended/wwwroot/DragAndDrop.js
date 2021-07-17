/**** 
 * DrUalcman 17 Jun 2021
 * Author: Sergi Ortiz Gomez
 * Version 1.2.5 BlazorInputFileExtended
 * License: Apache License, Version 2.0 
 *          SPDX short identifier: Apache-2.0
 ****/
var InputFileId;            //id for the inputfile into the page to setup the events for drag and drop
/** Export DragAndDrop to load the file dynamically */
export const DragAndDrop = {
    // Load draganddrop Javascripts
    Load: (dropId) => {
        let tag = document.getElementById('draganddrop-script');
        if (!tag) {
            tag = document.createElement('script');
            tag.id = 'draganddrop-script'
            tag.src = "/_content/BlazorInputFileExtended/DragAndDrop.js";
            var firstScriptTag = document.getElementsByTagName('script')[0];
            firstScriptTag.parentNode.insertBefore(tag, firstScriptTag);
        }
        InputFileId = dropId;
        setEvents();
    },
    // UnLoad draganddrop Javascripts
    UnLoad: () => {
        let tag = document.getElementById('draganddrop-script');
        try {
            if (tag) tag.remove();
            let d = document.getElementById(`Contaniner_${InputFileId}`);
            if (d) {
                d.removeEventListener('dragover', dragOverHandler, false);
                d.removeEventListener('drop', dropHandler, false);
            }
        } catch (e) {
            console.warn(e);
        }
    }
}
/*** Setup the events */
function setEvents() {
    let d = document.getElementById(`Contaniner_${InputFileId}`);
    if (d) {
        d.addEventListener('dragover', dragOverHandler, false);
        d.addEventListener('drop', dropHandler, false);
    }
}

function dropHandler(ev) {
    // Prevent default behavior (Prevent file from being opened)
    ev.preventDefault();
    //ev.stopPropagation();
    // send the files to the input file
    let inputfile = document.getElementById(InputFileId);
    inputfile.files = ev.dataTransfer.files;
    // Generate the change event to notify to Blazor have some change.
    try {
        let event = new Event('change');   // Create the event.
        inputfile.dispatchEvent(event);    // Dispatch the event.
    } catch (e) {
        console.warn(e);
    }
    // clean the drag and drop data
    removeDragData(ev)
}

function dragOverHandler(ev) {
    // Prevent default behavior (Prevent file from being opened)
    ev.preventDefault();
    ev.stopPropagation();
}

function removeDragData(ev) {
    if (ev.dataTransfer.items) {
        // Use DataTransferItemList interface to remove the drag data
        ev.dataTransfer.items.clear();
    } else {
        // Use DataTransfer interface to remove the drag data
        ev.dataTransfer.clearData();
    }
}