import React, { useState } from 'react';
import { Form, Typography, message } from 'antd';
import './index.css';
import CustomInput from '../../../Components/CustomInput';
import CustomPasswordInput from '../../../Components/CustomPasswordInput';
import CustomSelect from '../../../Components/CustomSelect';
import CustomButton from '../../../Components/CustomButton';
import { useCreateUser } from '../../../Connections/AppBackend/User/Index';

const { Title, Text } = Typography;

interface FormValues {
  email: string;
  userName: string;
  displayName?: string;
  passwordHash: string;
  dateOfBirth: {
    day: number;
    month: number;
    year: number;
  }
}

interface ErrorState {
  [key: string]: string | undefined;
}

const RegisterForm: React.FC = () => {
  const [messageApi, contextHolder] = message.useMessage();
  const [form] = Form.useForm<FormValues>();
  const [errors, setErrors] = useState<ErrorState>({});
  const [invalidDate, setInvalidDate] = useState<boolean>(false);
  const [day, setDay] = useState<number | undefined>();
  const [month, setMonth] = useState<number | undefined>();
  const [year, setYear] = useState<number | undefined>();
  const { mutate } = useCreateUser();


  const years = Array.from({ length: new Date().getFullYear() - 1979 }, (_, i) => 1980 + i);
  const months = [
    'January', 'February', 'March', 'April', 'May', 'June',
    'July', 'August', 'September', 'October', 'November', 'December'
  ].map((month, index) => ({ value: index + 1, label: month }));
  const days = Array.from({ length: 31 }, (_, i) => i + 1);

  const isValidDate = (y?: number, m?: number, d?: number): boolean => {
    if (!y || !m || !d) return true; // Không check khi chưa đủ

    const date = new Date(y, m - 1, d); // month: 0-based
    return (
      date.getFullYear() === y &&
      date.getMonth() === m - 1 &&
      date.getDate() === d
    );
  };

  const handleChange = (value: number | undefined, type: 'day' | 'month' | 'year') => {
    if (type === 'day') setDay(value);
    else if (type === 'month') setMonth(value);
    else if (type === 'year') setYear(value);
    const updatedDay = type === 'day' ? value : day;
    const updatedMonth = type === 'month' ? value : month;
    const updatedYear = type === 'year' ? value : year;
    const valid = isValidDate(updatedYear, updatedMonth, updatedDay);
    setInvalidDate(!valid);
    if (!valid) {
      setErrors({ ...errors, dateOfBirth: 'Invalid date' });
      form.setFields([
        {
          name: 'dateOfBirth',
          errors: valid ? [] : [''],
        },
      ]);
    }
    else {
      setErrors({ ...errors, dateOfBirth: undefined });
    }

  };


  const onFinish = (values: FormValues) => {
    if (!invalidDate) {
      setErrors({});
      const dateOfBirth = values.dateOfBirth
        ? new Date(values.dateOfBirth.year, values.dateOfBirth.month - 1, values.dateOfBirth.day)
        : undefined;
      const input = {
        email: values.email,
        userName: values.userName,
        displayName: values.displayName,
        passwordHash: values.passwordHash,
        dateOfBirth: dateOfBirth ? dateOfBirth.toISOString().slice(0, 10) : undefined,
      };
      mutate(input, {
        onSuccess: (response) => {
          messageApi
            .open({
              type: 'loading',
              content: 'Action in progress..',
              duration: 0.5,
            })
            .then(() => message.success("", 2.5))
        },
        onError: (err) => {
          messageApi
            .open({
              type: 'loading',
              content: 'Action in progress..',
              duration: 0.5,
            })
            .then(() => message.error(err.message, 2.5))
        },
      });
    }
  };

  const onFinishFailed = ({ errorFields }: { errorFields: any[] }) => {
    // Lưu lỗi vào state
    const newErrors: ErrorState = {};
    errorFields.forEach((field) => {
      newErrors[field.name[0]] = field.errors[0];
    });
    setErrors(newErrors);
  };

  return (
    <div className="register-container">
      {contextHolder}
      <div className='logo-container'>
        <div className="logo">
        </div>
        <div style={{ color: 'white', fontSize: 24, fontWeight: 'bold', marginLeft: 10 }}>
          KenVerse
        </div>
      </div>

      <div className="register-box">
        <Title level={3} style={{ color: 'white', textAlign: 'center' }}>
          Create account
        </Title>

        <Form
          form={form}
          name="register_demo_discord"
          layout="vertical"
          onFinish={onFinish}
          onFinishFailed={onFinishFailed}
          autoComplete="off"
          requiredMark={false}
        >
          {/* Input giả để đánh lừa Chrome autofill */}
          <input type="text" name="fakeuser" autoComplete="username" style={{ display: 'none' }} />
          <input type="password" name="fakepassword" autoComplete="current-password" style={{ display: 'none' }} />

          <Form.Item
            label={
              <span>
                <span style={{ fontSize: 12, fontWeight: 600 }}>EMAIL</span>{' '}<span style={{ color: 'rgb(245, 121, 118)' }}>*</span>
                {errors.email != null && (
                  <span style={{ color: 'rgb(245, 121, 118)', fontStyle: "italic", fontSize: 12 }}> - Invalid</span>
                )}
              </span>
            } name="email"
            rules={[{ required: true, message: '' }, { type: 'email', message: '' }]}
          >
            <CustomInput
              size="large"
              style={{
                backgroundColor: "#28282d",
                color: '#d0d1d3',
                borderColor: '#40444b',
              }}
            />
          </Form.Item>

          <Form.Item
            label={<span style={{ fontSize: 12, fontWeight: 600 }}>DISPLAY NAME</span>}
            name="displayName"
          >
            <CustomInput
              size="large"
              style={{
                backgroundColor: "#28282d",
                color: '#d0d1d3',
                borderColor: '#40444b',
              }}
            />
          </Form.Item>

          <Form.Item
            label={
              <span>
                <span style={{ fontSize: 12, fontWeight: 600 }}>USER NAME</span>{' '}<span style={{ color: 'rgb(245, 121, 118)' }}>*</span>
                {errors.userName != null && (
                  <span style={{ color: 'rgb(245, 121, 118)', fontStyle: "italic", fontSize: 12 }}> - Invalid</span>
                )}
              </span>
            } name="userName"
            rules={[{ required: true, message: '' }]}

          >
            <CustomInput
              size="large"
              style={{
                backgroundColor: "#28282d",
                color: '#d0d1d3',
                borderColor: '#40444b',
              }}
            />
          </Form.Item>

          <Form.Item
            label={
              <span>
                <span style={{ fontSize: 12, fontWeight: 600 }}>PASSWORD</span>{' '}<span style={{ color: 'rgb(245, 121, 118)' }}>*</span>
                {errors.passwordHash != null && (
                  <span style={{ color: 'rgb(245, 121, 118)', fontStyle: "italic", fontSize: 12 }}> Invalid</span>
                )}
              </span>
            }
            name="passwordHash"
            rules={[{ required: true, message: '' }]}
            help={null}
            style={{ textAlign: "left" }}
          >
            <CustomPasswordInput
              size="large"
              style={{
                backgroundColor: "#28282d",
                color: '#d0d1d3',
                borderColor: '#40444b',
              }}
              visibilityToggle={false}

            />
          </Form.Item>

          <Form.Item
            label={
              <span>
                <span style={{ fontSize: 12, fontWeight: 600 }}>DATE OF BIRTH</span>{' '}
                <span style={{ color: 'rgb(245, 121, 118)' }}>*</span>
                {errors.dateOfBirth != null && (
                  <span style={{ color: 'rgb(245, 121, 118)', fontStyle: "italic", fontSize: 12 }}> - Invalid</span>
                )}
              </span>
            }
            validateStatus={invalidDate ? 'error' : undefined}
            name={'dateOfBirth'}
            required
          >
            <div style={{ display: 'flex', justifyContent: 'space-between' }}>
              <Form.Item name={['dateOfBirth', 'day']} noStyle rules={[{ required: true, message: '' }]} >
                <CustomSelect
                  className='register-select-dob'
                  dropdownClassName="register-select-dob-dropdown"
                  size="large"
                  options={days.map(day => ({ value: day, label: day }))}
                  placeholder="Day"
                  showArrow={true}
                  customStyle={{ width: "30%" }}
                  dropdownStyle={{ backgroundColor: '#28282d' }}
                  showSearch
                  onChange={(value) => handleChange(value, 'day')}
                />
              </Form.Item>
              <Form.Item name={['dateOfBirth', 'month']} noStyle rules={[{ required: true, message: '' }]}>
                <CustomSelect
                  className='register-select-dob'
                  dropdownClassName="register-select-dob-dropdown"
                  size="large"
                  options={months}
                  placeholder="Month"
                  showArrow={true}
                  customStyle={{ width: "30%" }}
                  dropdownStyle={{ backgroundColor: '#28282d' }}
                  showSearch
                  onChange={(value) => handleChange(value, 'month')}
                />
              </Form.Item>
              <Form.Item name={['dateOfBirth', 'year']} noStyle rules={[{ required: true, message: '' }]}>
                <CustomSelect
                  className='register-select-dob'
                  dropdownClassName="register-select-dob-dropdown"
                  size="large"
                  options={years.map(year => ({ value: year, label: year }))}
                  placeholder="Year"
                  showArrow={true}
                  customStyle={{ width: "30%" }}
                  dropdownStyle={{ backgroundColor: '#28282d' }}
                  showSearch
                  onChange={(value) => handleChange(value, 'year')}
                />
              </Form.Item>
            </div>
          </Form.Item>


          <Form.Item>
            <CustomButton type="primary" htmlType="submit" block size="large" style={{ backgroundColor: '#5865f2' }}>
              Continue
            </CustomButton>
          </Form.Item>
        </Form>

        <div style={{ textAlign: 'left', marginTop: 16 }}>
          <Text type="secondary" style={{ color: "#b5bac1" }}>
            <a href="/Login" style={{ color: "#5865f2" }}>Already have an account?</a>
          </Text>
        </div>
      </div>
    </div >
  );
};

export default RegisterForm;
