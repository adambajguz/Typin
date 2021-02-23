String.prototype.isNullOrWhiteSpace = function () { return (!this || this.length === 0 || /^\s*$/.test(this)) }
