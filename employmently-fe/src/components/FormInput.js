import { useState } from "react";

function FormInput(props) {
    const [focused, setFocused] = useState(false);
    const { label, errorMessage, onChange, id, ...inputProps } = props;

    const handleFocus = (e) => {
        setFocused(true);
    };

    return (
        <div className="formInput">
            <input
                {...inputProps}
                onChange={onChange}
                // on click outside the input box, the input box will lose focus
                onBlur={handleFocus}
                onFocus={() =>
                    inputProps.name === "confirmPassword" && setFocused(true)
                }
                focused={focused.toString()}
            />
            <span className="login-span">{errorMessage}</span>
        </div>
    );
};

export default FormInput;