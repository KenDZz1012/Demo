export const spreadSearchQuery = (searchQuery: Record<string, unknown> = {}): string => {
  const queryParts: string[] = [];

  Object.keys(searchQuery).forEach((key) => {
    const value = searchQuery[key];

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
        queryParts.push(`${encodeURIComponent(key)}=${encodeURIComponent(String(v))}`);
      });
    } else {
      queryParts.push(`${encodeURIComponent(key)}=${encodeURIComponent(String(value))}`);
    }
  });

  return queryParts.length > 0 ? `?${queryParts.join('&')}` : '';
};
