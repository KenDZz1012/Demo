import React from 'react';
import { Input } from 'antd';
import { InputProps } from 'antd/es/input';

interface CustomInputProps extends InputProps {
    icon?: React.ReactNode;
    iconPosition?: 'left' | 'right';
    customStyle?: React.CSSProperties;
}

const CustomInput: React.FC<CustomInputProps> = ({
    value,
    onChange,
    placeholder,
    icon,
    iconPosition = 'left',
    customStyle,
    ...rest
}) => {
    return (
        <Input
            value={value}
            onChange={onChange}
            placeholder={placeholder}
            prefix={iconPosition === 'left' ? icon : undefined}
            suffix={iconPosition === 'right' ? icon : undefined}
            style={{
                ...customStyle, // cho phép ghi đè
            }}
            {...rest}
        />
    );
};

export default CustomInput;
