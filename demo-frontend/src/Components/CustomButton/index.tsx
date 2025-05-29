import React from 'react';
import { Button } from 'antd';
import { ButtonProps } from 'antd/lib/button';

interface CustomButtonProps extends ButtonProps {
    rounded?: boolean;
    bgColor?: string;
    textColor?: string;
    hoverColor?: string;
}

const CustomButton: React.FC<CustomButtonProps> = ({
    children,
    rounded = true,
    bgColor = '#5865F2',
    textColor = 'white',
    hoverColor = '#4752C4',
    style,
    ...rest
}) => {
    return (
        <Button
            {...rest}
            style={{
                transition: 'all 0.3s ease',
                ...style,
            }}
            onMouseEnter={(e) => {
                (e.currentTarget as HTMLElement).style.backgroundColor = hoverColor;
            }}
            onMouseLeave={(e) => {
                (e.currentTarget as HTMLElement).style.backgroundColor = bgColor;
            }}
        >
            {children}
        </Button>
    );
};

export default CustomButton;
