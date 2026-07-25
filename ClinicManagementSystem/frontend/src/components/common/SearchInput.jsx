import "./SearchInput.css";

export function SearchInput({ value, onChange, placeholder = "Search..." }) {
  return (
    <div className="search-input">
      <span className="search-input__icon" aria-hidden="true">
        ⌕
      </span>
      <input
        type="text"
        value={value}
        onChange={(e) => onChange(e.target.value)}
        placeholder={placeholder}
      />
    </div>
  );
}
