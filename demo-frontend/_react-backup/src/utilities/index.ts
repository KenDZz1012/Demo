export const spreadSearchQuery = (searchQuery: Record<string, any> = {}) => {
    const queryParts: string[] = [];

    Object.keys(searchQuery).forEach((key) => {
        const value = searchQuery[key];

        // Bỏ qua nếu null, undefined, "", hoặc mảng rỗng
        if (
            value === null ||
            value === undefined ||
            (typeof value === 'string' && value.trim() === '') ||
            (Array.isArray(value) && value.length === 0)
        ) {
            return;
        }

        if (Array.isArray(value)) {
            value.forEach((v) => {
                queryParts.push(`${encodeURIComponent(key)}=${encodeURIComponent(v)}`);
            });
        } else {
            queryParts.push(`${encodeURIComponent(key)}=${encodeURIComponent(value)}`);
        }
    });

    return queryParts.length > 0 ? `?${queryParts.join('&')}` : '';
};


export function generateUUID() {
    if (typeof crypto !== 'undefined' && crypto.randomUUID) {
        return crypto.randomUUID();
    }

    return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
        const r = Math.random() * 16 | 0;
        const v = c === 'x' ? r : (r & 0x3 | 0x8);
        return v.toString(16);
    });
}
