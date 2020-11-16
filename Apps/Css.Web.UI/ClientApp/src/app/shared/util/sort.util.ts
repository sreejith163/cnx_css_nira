export class Sort {

    private sortOrder = 1;
    private collator = new Intl.Collator(undefined, {
        numeric: true,
        sensitivity: 'base',
    });

    constructor() { }

    startSort(property, order, type = '') {
        if (order === 'desc') {
            this.sortOrder = -1;
        }
        return (a, b) => {
            if (type === 'date') {
                return this.sortData(new Date(a[property]), new Date(b[property]));
            } else if (type === 'array') {
                return this.collator.compare(a[property][0].id, b[property][0].id) * this.sortOrder;
            }
            else {
                const sd = this.collator.compare(a[property], b[property]) * this.sortOrder;
                return this.collator.compare(a[property], b[property]) * this.sortOrder;
            }
        };
    }

    private sortData(a, b) {
        if (a < b) {
            return -1 * this.sortOrder;
        } else if (a > b) {
            return 1 * this.sortOrder;
        } else {
            return 0 * this.sortOrder;
        }
    }
}
