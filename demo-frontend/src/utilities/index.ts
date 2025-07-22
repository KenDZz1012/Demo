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
