import React from 'react';
import { Select } from 'antd';
import type { SelectProps } from 'antd';
import { DownOutlined } from '@ant-design/icons';

export interface CustomSelectProps extends SelectProps<any> {
    showArrow?: boolean;
    customStyle?: React.CSSProperties;
}

const CustomSelect: React.FC<CustomSelectProps> = ({
    value,
    onChange,
    options,
    placeholder,
    showArrow = true,
    customStyle,
    ...rest
}) => {
    return (
        <Select
            value={value}
            onChange={onChange}
            options={options}
            placeholder={placeholder}
            suffixIcon={showArrow ? <DownOutlined /> : null}
            style={{
                ...customStyle,
            }}
            {...rest}
        />
    );
};

export default CustomSelect;
