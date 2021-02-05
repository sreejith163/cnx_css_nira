function toggleSidebar() {
  if ($('body').hasClass('sidebar-collapse')) {
    $('body').removeClass('sidebar-collapse');
  } else {
    $('body').addClass('sidebar-collapse ');
  }
};

var startRowIndex = null;
var startCellIndex = null;

function setRowCellIndex(cellId) {
  const clickedCell = $('#' + cellId);
  startCellIndex = clickedCell.index();
  startRowIndex = clickedCell.parent().index();
}

function setManagerRowCellIndex(cellIndex, rowIndex) {
  startCellIndex = cellIndex + 2;
  startRowIndex = rowIndex;
}

function highlightSelectedCells(tableId, cellId) {
  const table = $('#' + tableId);
  const cell = $('#' + cellId);

  const row = cell.parent();
  const cellIndex = cell.index();
  const rowIndex = row.index();

  let rowStart;
  let rowEnd;
  let cellStart;
  let cellEnd;
  if (rowIndex < startRowIndex) {
    rowStart = rowIndex;
    rowEnd = startRowIndex;
  } else {
    rowStart = startRowIndex;
    rowEnd = rowIndex;
  }

  if (cellIndex < startCellIndex) {
    cellStart = cellIndex;
    cellEnd = startCellIndex;
  } else {
    cellStart = startCellIndex;
    cellEnd = cellIndex;
  }

  for (let i = rowStart; i <= rowEnd; i++) {
    const rowCells = table.find('tr').eq(i).find('td');
    for (let j = cellStart; j <= cellEnd; j++) {
      rowCells.eq(j).addClass('cell-selected');
    }
  }
}

function highlightManagerSelectedCells(tableId, cellId) {
  const table = $('#' + tableId);
  const cell = $('#' + cellId);

  const row = cell.parent();
  const cellIndex = cell.index();
  const rowIndex = row.index() + 1;

  let rowStart;
  let rowEnd;
  let cellStart;
  let cellEnd;
  if (rowIndex < startRowIndex) {
    rowStart = rowIndex;
    rowEnd = startRowIndex;
  } else {
    rowStart = startRowIndex;
    rowEnd = rowIndex;
  }

  if (cellIndex < startCellIndex) {
    cellStart = cellIndex;
    cellEnd = startCellIndex;
  } else {
    cellStart = startCellIndex;
    cellEnd = cellIndex;
  }

  for (let i = rowStart; i <= rowEnd; i++) {
    const rowCells = table.find('tr').eq(i).find('td');
    for (let j = cellStart; j <= cellEnd; j++) {
      rowCells.eq(j).addClass('cell-selected');
    }
  }
}

function highlightCell(cellId, className) {
  const cell = $('#' + cellId);
  cell.addClass(className);
}

function highlightManagerCell(cellId, className) {
  const cell = $('#' + cellId);
  cell.addClass(className);
}

function removeTable(tableId) {
  const table = $('#' + tableId);
  table.remove();
}

function removeHighlightedCells(tableId, className) {
  const table = $('#' + tableId);
  table.find('.' + className).removeClass(className);
}
