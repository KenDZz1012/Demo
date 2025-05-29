import React, { useState } from 'react';
import { Input } from 'antd';
import { EyeInvisibleOutlined, EyeTwoTone, LockOutlined } from '@ant-design/icons';
import { PasswordProps } from 'antd/es/input';

interface CustomPasswordInputProps extends PasswordProps {
    customStyle?: React.CSSProperties;
    showIconRender?: boolean;
}

const CustomPasswordInput: React.FC<CustomPasswordInputProps> = ({
    value,
    onChange,
    placeholder,
    customStyle,
    visibilityToggle = true,
    showIconRender = false,
    ...rest
}) => {
    return (
        <Input.Password
            value={value}
            onChange={onChange}
            placeholder={placeholder}
            visibilityToggle={visibilityToggle}
            iconRender={(visible) => (visible ? <EyeTwoTone /> : <EyeInvisibleOutlined />)}
            prefix={showIconRender ? <LockOutlined style={{ color: '#999' }} /> : null}
            style={{
                borderRadius: 8,
                backgroundColor: '#f5f5f5',
                border: '1px solid #d9d9d9',
                padding: '0.5rem',
                ...customStyle,
            }}
            {...rest}
        />
    );
};

export default CustomPasswordInput;
